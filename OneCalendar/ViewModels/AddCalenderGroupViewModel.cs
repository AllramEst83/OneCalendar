using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels
{
    public class AddCalenderGroupViewModel
    {
        public string GroupName { get; set; }
        public List<Guid> GroupUsers{ get; set; }
    }
}
