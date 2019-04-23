using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class CalenderTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public User TaskCreatedByUser { get; set; }
        public IEnumerable<EditedByUser> Edited { get; set; }
    }
}
