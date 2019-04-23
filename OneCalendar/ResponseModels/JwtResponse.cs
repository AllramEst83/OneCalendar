using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OneCalendar.ResponseModels
{
    public class JwtResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string Auth_Token { get; set; }
        public int Expires_In { get; set; }
    }
}
