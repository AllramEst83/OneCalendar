﻿using TokenValidation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneCalendar.Context;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.ResponseModels;
using OneCalendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OneCalendar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalenderController : ControllerBase
    {
        public CalenderController(ICalenderService calenderService, IAccountService accountService)
        {
            CalenderService = calenderService;
            AccountService = accountService;
        }

        public CalenderContext CalenderContext { get; }
        public ICalenderService CalenderService { get; }
        public IAccountService AccountService { get; }


        //DeleteGroup
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteGroup(DeleteGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            List<int> groups = model.Groups;
            bool groupExists = await CalenderService.GroupsExistsById(groups);
            if (!groupExists)
            {
                return new JsonResult(new DeleteGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.NotFound,
                    Error = "conflict",
                    Description = "Gruppnament hittades inte."
                });
            }

            bool deleteGroupsResult = await CalenderService.DeleteGroupsById(groups);
            if (!deleteGroupsResult)
            {
                return new JsonResult(new DeleteGroupResponse
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unable_to_delete_calender_group",
                    Description = "Unable to delete clendergroup."
                });

            }

            return new JsonResult(new DeleteGroupResponse
            {
                Content = new { },
                StatusCode = HttpStatusCode.OK,
                Error = "successfully_delete_calender_group",
                Description = "Successfully deleted calendergroup."
            });
        }

        //AddGroup
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpPost]
        public async Task<ActionResult<string>> AddGroup(AddCalenderGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            string groupName = model.GroupName.Trim();
            bool groupNameExists = await CalenderService.GroupExistsByName(groupName);
            if (groupNameExists)
            {
                return new JsonResult(new AddUserToGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.Conflict,
                    Error = "conflict",
                    Description = "Gruppnament finns redan. välj ett annat gruppnamn."
                });
            }

            bool addGroupResult = await CalenderService.AddGroup(model);
            if (!addGroupResult)
            {
                return new JsonResult(new AddCalenderGroupResponse
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unable_to_create calender_group",
                    Description = "Unable to add calendergroup at this time."
                });
            }

            return new JsonResult(new AddCalenderGroupResponse
            {
                Content = model,
                StatusCode = HttpStatusCode.OK,
                Error = "calender_group_created",
                Description = "Calendergroup successfully created."
            });
        }

        //DeleteEvent
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIEditUser)]
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteEvent(DeleteEventViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            bool DeleteEventReuslt = await CalenderService.DeleteCalenderEvent(model);
            if (!DeleteEventReuslt)
            {
                return new JsonResult(new DeleteEventResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unable_to_delete_event",
                    Description = "Unable to add event at this time."
                });
            }

            return JsonConvert
               .SerializeObject(
                new DeleteEventResponseModel()
                {
                    Content = DeleteEventReuslt,
                    StatusCode = HttpStatusCode.OK,
                    Error = "event_successfully_deleted",
                    Description = "Event successfully deleted."
                },
               new JsonSerializerSettings
               {
                   Formatting = Formatting.Indented,
                   ContractResolver = new CamelCasePropertyNamesContractResolver()
               });
        }

        //UpdateEvent
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIEditUser)]
        [HttpPut]
        public async Task<ActionResult<string>> UpdateEvent(UpdateEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            bool updateEvenrResult = await CalenderService.UpdateCalenderEvent(model);
            if (!updateEvenrResult)
            {
                return new JsonResult(new UpdateEventResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unable_to_add_event",
                    Description = "Unable to add event at this time."
                });
            }


            return JsonConvert
           .SerializeObject(
          new UpdateEventResponseModel()
          {
              Content = model,
              StatusCode = HttpStatusCode.OK,
              Error = "event_successfully_updated",
              Description = "Event successfully updated."
          },
           new JsonSerializerSettings
           {
               Formatting = Formatting.Indented,
               ContractResolver = new CamelCasePropertyNamesContractResolver()
           });
        }

        //AddEvent
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIEditUser)]
        [HttpPost]
        public async Task<ActionResult<string>> AddEvent(AddEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new OkObjectResult(new { messsage = "build error" });
            }

            bool addEventResult = await CalenderService.AddCalenderEvent(model);
            if (addEventResult == false)
            {
                return new JsonResult(new AddEventResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unable_to_add_event",
                    Description = "Unable to add event at this time."
                });
            }

            return JsonConvert
               .SerializeObject(
              new AddEventResponseModel()
              {
                  Content = model,
                  StatusCode = HttpStatusCode.OK,
                  Error = "event_successfully_added",
                  Description = "Event successfully added."
              },
               new JsonSerializerSettings
               {
                   Formatting = Formatting.Indented,
                   ContractResolver = new CamelCasePropertyNamesContractResolver()
               });
        }

        //GetInfo
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetInfo()
        {
            List<ShortHandCalanderGroup> groups = new List<ShortHandCalanderGroup>();
            List<ShortHandUsers> users = new List<ShortHandUsers>();
            List<Roles> roles = new List<Roles>();
            UsersAndGroups usersAndGroupsToClient;
            try
            {
                groups = await CalenderService.GetCalenderGroups();
                users = await AccountService.GetAllUsers();
                roles = await AccountService.GetRoles();
            }
            catch (Exception ex)
            {

                //Skriv felmeddelande som frontend kan hantera
            }
            finally
            {
                usersAndGroupsToClient = new UsersAndGroups()
                {
                    Users = users,
                    Groups = groups,
                    Roles = roles
                };
            }

            return new JsonResult(usersAndGroupsToClient);
        }

        //AssignRole
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpPut]
        public async Task<ActionResult<string>> AssignRole([FromBody]AssignRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "ModelState is invalid.",
                    Error = "modelState inValid"
                });
            }

            string userId = model.Id.ToString();
            string role = model.Role.ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "UserId or role can not be empty.",
                    Error = "userid_or_role_can_not_be_empty"
                });
            }

            User user = await AccountService.GetUserById(userId);
            bool roleExists = await AccountService.RoleExists(role);

            if (user == null || !roleExists)
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "User or role does not exits",
                    Error = "bad_request"
                });
            }

            bool userHasRole = await AccountService.UserHasRole(user, role);
            if (userHasRole)
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.Conflict,
                    Description = $"User already has role: {role} assigned",
                    Error = "conflict"
                });
            }

            IdentityResult assigRoleToUserResult = await AccountService.AddRoleToUser(user, role);

            return new OkObjectResult(new AssignRoleResponseModel()
            {
                Content = assigRoleToUserResult,
                StatusCode = HttpStatusCode.OK,
                Description = $"User: {user.Email} has been assigned the role: {role}",
                Error = "no_error"
            });
        }

        //UnAssignRole
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpPut]
        public async Task<ActionResult<string>> UnAssignRole([FromBody]UnAssignRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new UnAssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "ModelState is invalid.",
                    Error = "modelState inValid"
                });
            }

            string userId = model.Id.ToString();
            string role = model.Role.ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "UserId or role can not be empty.",
                    Error = "userid_or_role_can_not_be_empty"
                });
            }

            User user = await AccountService.GetUserById(userId);
            bool roleExists = await AccountService.RoleExists(role);

            if (user == null || !roleExists)
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Description = "User or role does not exits",
                    Error = "bad_request"
                });
            }

            bool userHasRole = await AccountService.UserHasRole(user, role);
            if (!userHasRole)
            {
                return new JsonResult(new AssignRoleResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.Conflict,
                    Description = $"User does not have role: {role} assigned",
                    Error = "conflict"
                });
            }

            IdentityResult assigRoleToUserResult = await AccountService.RemoveRoleFromUser(user, role);

            return new OkObjectResult(new AssignRoleResponseModel()
            {
                Content = assigRoleToUserResult,
                StatusCode = HttpStatusCode.OK,
                Description = $"Role: {role} has been romoved from user: {user.Email}",
                Error = "no_error"
            });
        }

        //AddUserToGroup
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpPost]
        public async Task<ActionResult<string>> AddUserToGroup(AddUserToGroupViewModel model)
        {
            string userId = model.UserId.ToString();
            int groupId = model.GroupId;

            bool userExists = await AccountService.UserExistById(userId);
            if (!userExists)
            {
                return new JsonResult(new AddUserToGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.NotFound,
                    Error = "not_found",
                    Description = "Användaren går tyvärr inte att hitta."
                });
            }

            bool userExistsInGroup = await CalenderService.UserExistsInGroup(groupId, userId);
            if (userExistsInGroup)
            {
                return new JsonResult(new AddUserToGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "bad_request",
                    Description = "Användaren finns readn i gruppen."
                });
            }
            CalenderGroup group = await CalenderService.AddUserToCalenderGroup(groupId, userId);
            if (group == null)
            {
                return new JsonResult(new AddUserToGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "bad_request",
                    Description = "Kunde inte lägga till användaren till gruppen"
                });
            }
            return new JsonResult(new AddUserToGroupResponse()
            {
                Content = group.ListOfUserIds,
                StatusCode = HttpStatusCode.OK,
                Error = "no_error",
                Description = "Användaren har lagts till i gruppen."
            });
        }

        //RemoveUserFromGroup
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpDelete]
        public async Task<ActionResult<string>> RemoveUserFromGroup(RemoveUserFromGroupViewModel model)
        {
            string userId = model.UserId.ToString();
            int groupId = model.GroupId;

            bool userExists = await AccountService.UserExistById(userId);
            bool groupExists = await CalenderService.GroupExistById(groupId);
            if (!userExists && !groupExists)
            {
                return new JsonResult(new RemoveUserFromGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.NotFound,
                    Error = "not_found",
                    Description = "Användaren eller gruppen går tyvärr inte att hitta."
                });
            }

            bool userInGroup = await CalenderService.UserExistsInGroup(groupId, userId);
            if (!userInGroup)
            {
                return new JsonResult(new RemoveUserFromGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.NotFound,
                    Error = "not_found",
                    Description = "Användaren finns inte i den angivna gruppen."
                });
            }

            CalenderGroup calenderGroup = await CalenderService.RemoveUserFromCalenderGroup(groupId, userId);
            if (calenderGroup == null)
            {
                return new JsonResult(new RemoveUserFromGroupResponse()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "bad_request",
                    Description = "Kunde inte lägga till användaren till gruppen"
                });
            }

            return new JsonResult(new RemoveUserFromGroupResponse()
            {
                Content = calenderGroup.ListOfUserIds,
                StatusCode = HttpStatusCode.OK,
                Error = "no_error",
                Description = "Användaren har tagits bort i gruppen."
            });
        }

        //GetTasksByUserId
        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIEditUser)]
        [HttpGet]
        public ActionResult<string> GetTasksByUserId(Guid id)
        {
            Guid userId = id;
            IEnumerable<GroupResponse> groupsAndEvents = CalenderService.GetGroupsAndEvents(userId);

            //Serialize groupAndEvents
            return JsonConvert
                .SerializeObject(
                groupsAndEvents,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}