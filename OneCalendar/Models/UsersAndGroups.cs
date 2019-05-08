using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class UsersAndGroups
    {
        public List<ShortHandCalanderGroup> Groups { get; set; }
        public List<ShortHandUsers> Users { get; set; }
    }
}
