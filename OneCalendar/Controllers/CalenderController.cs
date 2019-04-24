using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public List<string> GetTasksByUserId(int id)
        {

            List<string> list = new List<string>() { "test", "test", "test" };
            return list;
        }
    }
}