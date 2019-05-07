using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CaseSolutionsTokenValidationParameters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneCalendar.Context;
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
        public ICalenderService CalenderService { get; }

        private readonly JwtIssuerOptions _jwtOptions;
        private readonly AppSettings _appsettings;

        public AuthController(
            IAccountService accountService,
            IMapper mapper,
            UserContext context,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
               IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<AppSettings> appsettings,
            ICalenderService calenderService)
        {
            AccountService = accountService;
            Mapper = mapper;
            Context = context;
            RoleManager = roleManager;
            JwtService = jwtService;
            CalenderService = calenderService;
            _jwtOptions = jwtOptions.Value;
            _appsettings = appsettings.Value;
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

            int parsedGroupId;
            string groupId = model.GroupId.Trim();
            if (!int.TryParse(groupId, out parsedGroupId))
            {
                return new JsonResult(new SignupResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "group_id_can_not_be_empty",
                    Description = "GroupId can not be empty."
                });
            }

            CalenderGroup addUserToCalenderGroupResult = CalenderService.AddUserToCalenderGroup(parsedGroupId, userIdentity.Id);
            if (addUserToCalenderGroupResult == null)
            {
                RedirectToAction("DeleteUser", userIdentity.Id);

                return new JsonResult(new SignupResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "no_group_with_matching_id",
                    Description = "No group with matching id."
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

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            User user = await AccountService.GetUserById(model.Id);
            if (user == null)
            {
                return new JsonResult(
                    new DeleteUserResponse()
                    {
                        Content = new { },
                        StatusCode = HttpStatusCode.NotFound,
                        Error = "user_not_found",
                        Description = "User is not found."
                    });
            }

            IList<string> rolesForUser = await AccountService.GetUserRoles(user);
            if (rolesForUser == null)
            {
                return new JsonResult(
                    new DeleteUserResponse()
                    {
                        Content = new { },
                        StatusCode = HttpStatusCode.NotFound,
                        Error = "roles_not_found",
                        Description = "Roles for user is not found."
                    });
            }

            if (rolesForUser.Any())
            {
                IdentityResult removeRolesFromUserResult = await AccountService.RemoveRolesFromUser(user, rolesForUser);

                if (!removeRolesFromUserResult.Succeeded)
                {
                    return new JsonResult(
                       new DeleteUserResponse()
                       {
                           Content = new { },
                           StatusCode = HttpStatusCode.UnprocessableEntity,
                           Error = "Unable to complete delete opretaion of user related roles.",
                           Description = "Roles realted to the current user could not be removed at this time.",
                       });
                }

                IdentityResult removeUserResult = await AccountService.DeleteUser(user);
                if (!removeUserResult.Succeeded)
                {
                    IdentityResult reAddRolesToUserResult = await AccountService.AddRolesToUser(user, rolesForUser);

                    if (reAddRolesToUserResult.Succeeded)
                    {
                        return new JsonResult(new DeleteUserResponse()
                        {
                            Content = new { },
                            StatusCode = HttpStatusCode.UnprocessableEntity,
                            Description = "User was not deleted. The delete task could not be completed at this time.",
                            Error = "Unable to complete delete opretaion."
                        });
                    }
                    return new JsonResult(new DeleteUserResponse()
                    {
                        Content = new { id = user.Id, email = user.Email },
                        StatusCode = HttpStatusCode.UnprocessableEntity,
                        Description = "User was not deleted. The delete task could not be completed at this time. User has no roles assign. Pleas add roles to user for access.",
                        Error = "Unable to complete delete opretaion. User does not have any roles"
                    });

                }

            }

            return new OkObjectResult(new DeleteUserResponse()
            {
                Content = new { id = user.Id, email = user.Email },
                StatusCode = HttpStatusCode.OK,
                Description = "user_deleted",
                Error = "User hase successfully been deleted"
            });
        }

        //Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string formatedResponse = JsonConvert.SerializeObject(new LoginResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "bad_request",
                    Description = "Bad Request."
                }, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                return new JsonResult(formatedResponse);
            }

            ClaimsIdentity identity = await JwtService.GetClaimsIdentity(model.UserName, model.Password);
            if (identity == null)
            {

                string formatedResponse = JsonConvert.SerializeObject(new LoginResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.Unauthorized,
                    Error = "login_failure",
                    Description = "Faild to login. Please verify your username or password."

                }, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                return new JsonResult(formatedResponse);
            }

            string jwtResponse = await JwtService
                .GenerateJwt(identity, JwtService, model.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() });

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



        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpPost]
        public ActionResult<object> RevalidateToken([FromQuery] string Authorization)
        {
            string token = Authorization.Substring(7, Authorization.Length - 7);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            bool isAuthenticated = principal.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                return new { isValid = isAuthenticated, userId = 0, userName = 0 };
            }

            JwtSecurityToken jwtToken = new JwtSecurityToken(token);
            string subjectName = jwtToken.Claims.Single(c => c.Type == "sub").Value;
            string subjectId = jwtToken.Claims.Single(c => c.Type == "id").Value;
            //RevalidateToken manually
            //https://stackoverflow.com/a/50206750/6804444

            return new { IsAuthenticated = isAuthenticated, userId = subjectId, userName = subjectName };
        }


        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = _appsettings.Issuer,
                ValidAudience = _appsettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsettings.Secret))
            };
        }

    }
}