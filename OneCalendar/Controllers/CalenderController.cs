using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OneCalendar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalenderController : ControllerBase
    {
        public CalenderController()
        {

        }

        [HttpGet]
        public ActionResult<string> GetTasksByUserId(int id)
        {
            List<string> list = new List<string>() { "test", "test", "test" };
            return JsonConvert.SerializeObject(list, new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}