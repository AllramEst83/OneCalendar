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
        public int TaskCreatedByUserId { get; set; } //<--Manually insert UserId from Auth DB
        public IEnumerable<EditedByUser> Edited { get; set; }
    }
}
