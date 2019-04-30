using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class CalenderTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TaskDescription { get; set; }
        public string CreatedBy { get; set; } //<--Manually insert UserId from Auth DB
        public IEnumerable<EditedByUser> Edited { get; set; }
    }
}
