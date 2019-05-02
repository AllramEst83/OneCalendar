using OneCalendar.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Models
{
    public class GroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<TaskResponse> Events { get; set; }
    }
}
