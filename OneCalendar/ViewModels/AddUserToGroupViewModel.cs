using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels
{
    public class AddUserToGroupViewModel
    {
        public Guid UserId { get; set; }
        public int GroupId { get; set; }
    }
}
