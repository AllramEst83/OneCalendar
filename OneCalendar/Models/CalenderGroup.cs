using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class CalenderGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CalenderTask> CalenderTasks { get; set; }


        private static readonly char delimiter = ';';
        private string _ids;
        [NotMapped]
        public string[] ListOfUserIds
        {
            get { return _ids.Split(delimiter); }
            set
            {
                _ids = string.Join($"{delimiter}", value);
            }
        }
    }
}
