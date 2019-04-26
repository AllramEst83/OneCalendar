using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace OneCalendar.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
    }
}