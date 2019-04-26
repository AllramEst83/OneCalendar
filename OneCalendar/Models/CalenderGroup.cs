using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class CalenderGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CalenderTask> CalenderTasks { get; set; }
    }
}
