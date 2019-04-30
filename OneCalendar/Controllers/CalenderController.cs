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
            var groupAndUserIds =
                  CalenderContext.CalenderGroups.Select(x => new { x.Id, x.ListOfUserIds }).ToList();
            foreach (var item in groupAndUserIds)
            {
                for (int i = 0; i < item.ListOfUserIds.Length; i++)
                {
                    if (item.ListOfUserIds[i].Equals(userId.ToString()))
                    {
                        groupIds.Add(item.Id);
                    }
                }
            }

            //var allCalenderTasks = CalenderContext.CalenderTasks.Where(x=> groupIds.Contains(x)

            return JsonConvert.SerializeObject(new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}