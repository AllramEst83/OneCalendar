using System;
using System.Collections.Generic;
using System.Linq;
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