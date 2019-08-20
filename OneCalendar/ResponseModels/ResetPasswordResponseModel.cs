using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OneCalendar.ResponseModels
{
    public class ResetPasswordResponseModel
    {
        public object Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string Description { get; set; }
    }
}
