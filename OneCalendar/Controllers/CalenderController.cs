﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CaseSolutionsTokenValidationParameters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneCalendar.Context;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.ResponseModels;
using OneCalendar.ViewModels;

namespace OneCalendar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalenderController : ControllerBase
    {
        public CalenderController(ICalenderService calenderService)
        {
            CalenderService = calenderService;
        }

        public CalenderContext CalenderContext { get; }
        public ICalenderService CalenderService { get; }

        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
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
                return new JsonResult(new AddEventResponseModel()
                {
                    Content = new { },
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Error = "unable_to_delete_event",
                    Description = "Unable to add event at this time."
                });
            }

            return JsonConvert
               .SerializeObject(
                new AddEventResponseModel()
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

        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
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


        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
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

        [HttpGet]
        public ActionResult<string> GetAllGroups()
        {
            List<ShortHandCalanderGroup> groups = CalenderService.GetCalenderGroups();

            //Serialize groupAndEvents
            return JsonConvert
                .SerializeObject(
                groups,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }


        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
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