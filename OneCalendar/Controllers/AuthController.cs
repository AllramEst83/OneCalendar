using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CaseSolutionsTokenValidationParameters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OneCalendar.Data;
using OneCalendar.Helpers.Settings;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.ResponseModels;
using OneCalendar.ViewModels;

namespace OneCalendar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAccountService AccountService { get; }
        public IMapper Mapper { get; }
        public UserContext Context { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public IJwtService JwtService { get; }
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(
            IAccountService accountService,
            IMapper mapper,
            UserContext context,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            AccountService = accountService;
            Mapper = mapper;
            Context = context;
            RoleManager = roleManager;
            JwtService = jwtService;
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Ping()
        {
            return new JsonResult(new { message = "AuthController is online!" });
        }

        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpGet]
        public IActionResult AdminAuthTest()
        {
            return new OkObjectResult(new { message = "Adimn authentication works" });
        }

        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIEditUser)]
        [HttpGet]
        public IActionResult EditAuthTest()
        {
            return new OkObjectResult(new { message = "Edit authentication works" });
        }

        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPICommonUser)]
        [HttpGet]
        public IActionResult CommonAuthTest()
        {
            return new OkObjectResult(new { message = "Common authentication works" });
        }

        //Signup
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            string userEmail = model.Email.Trim();
            if (await AccountService.UserExistByUserName(userEmail))
            {
                return new JsonResult(new SignupResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.Conflict,
                    Error = "user_exists",
                    Description = "User already exists. Please use another email."
                });
            }

            string userRole = model.Role.Trim();
            if (!await AccountService.RoleExists(userRole))
            {
                return new JsonResult(new SignupResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.NotFound,
                    Error = "role_not_found",
                    Description = "Role is not found."
                });
            }

            string password = model.Password.Trim();
            User userIdentity = Mapper.Map<User>(model);

            IdentityResult addUserResult = await AccountService.CreateUser(userIdentity, password);
            if (!addUserResult.Succeeded)
            {
                return new JsonResult(new SignupResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unprocessable_entity",
                    Description = "User could not be created."
                });
            }

            IdentityResult addRoleResult = await AccountService.AddRoleToUser(userIdentity, userRole);
            if (!addRoleResult.Succeeded)
            {
                return new JsonResult(new SignupResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unprocessable_entity",
                    Description = "Unable to link role to user."
                });
            }

            await Context.SaveChangesAsync();

            IList<string> userRoles = await AccountService.GetRolesForUser(userIdentity);

            EndUser endUser = Mapper.Map<EndUser>(userIdentity);

            return new OkObjectResult(new SignupResponseModel()
            {
                Content = new { user = endUser, userRoles = userRoles },
                StatusCode = HttpStatusCode.OK,
                Error = "no_error",
                Description = "User has successfully been created."
            });
        }

        //Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new LoginResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "bad_request",
                    Description = "Bad Request."
                });
            }

            ClaimsIdentity identity = await JwtService.GetClaimsIdentity(model.UserName, model.Password);
            if (identity == null)
            {
                return new JsonResult(new LoginResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.Unauthorized,
                    Error = "login_failure",
                    Description = "Faild to login. Please verify your username or password."

                });
            }

            string jwtResponse = await JwtService
                .GenerateJwt(identity, JwtService, model.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            return new OkObjectResult(jwtResponse);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SeedRoles()
        {
            bool result = await RoleManager.RoleExistsAsync(TokenValidationConstants.Roles.AdminAccess);
            List<IdentityRole> listOfRoles = null;

            if (!result)
            {
                listOfRoles = new List<IdentityRole>()
                {
                    new IdentityRole(){Name = TokenValidationConstants.Roles.AdminAccess},
                    new IdentityRole(){Name = TokenValidationConstants.Roles.EditUserAccess},
                    new IdentityRole(){Name = TokenValidationConstants.Roles.CommonUserAccess},
                };

                foreach (IdentityRole role in listOfRoles)
                {
                    await RoleManager.CreateAsync(role);
                    Context.SaveChanges();
                }

            }

            return new JsonResult(new { message = "Roles have been seeded successfully.", Roles = listOfRoles });
        }
    }
}