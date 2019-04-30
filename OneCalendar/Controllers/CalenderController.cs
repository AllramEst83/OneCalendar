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
using OneCalendar.Models;
using OneCalendar.ResponseModels;

namespace OneCalendar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalenderController : ControllerBase
    {
        public CalenderController(CalenderContext calenderContext)
        {
            CalenderContext = calenderContext;
        }

        public CalenderContext CalenderContext { get; }

        [Authorize(Policy = TokenValidationConstants.Policies.AuthAPIAdmin)]
        [HttpGet]
        public ActionResult<string> GetTasksByUserId(Guid id)
        {
            Guid userId = id;
            List<int> groupIds = new List<int>();
            List<CalenderGroup> allGroups =
                  CalenderContext.CalenderGroups.ToList();

            foreach (var item in allGroups)
            {
                int position = Array.IndexOf(item.ListOfUserIds, userId.ToString());
                if (position > -1)
                {
                    groupIds.Add(item.Id);
                }
            }

            List<CalenderGroup> userGroups = CalenderContext.CalenderGroups.Where(x => groupIds.Contains(x.Id)).Include(i => i.CalenderTasks).ToList();

            var groupsAndEvents = userGroups.Select(x =>
            new
            {
                groupId = x.Id,
                groupName = x.Name,
                events = x.CalenderTasks.Select(p =>
                new TaskResponse()
                {
                    Id = p.Id,
                    Title = p.TaskName,
                    Start = p.StartDate,
                    End = p.EndDate,
                    AllDay = false
                }).ToList()

            });

            return JsonConvert.SerializeObject(groupsAndEvents, new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}