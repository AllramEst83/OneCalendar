using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ViewModels
{
    public class AddEventViewModel
    {
        public Guid UserId { get; set; }
        public string UserName{ get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string EventColor { get; set; }
        public int GroupId { get; set; }
    }
}
