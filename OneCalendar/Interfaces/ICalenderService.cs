using OneCalendar.Context;
using OneCalendar.Models;
using OneCalendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Interfaces
{
    public interface ICalenderService
    {
        CalenderContext CalenderContext { get; }

        List<ShortHandCalanderGroup> GetCalenderGroups();
        IEnumerable<GroupResponse> GetGroupsAndEvents(Guid userId);
        CalenderGroup AddUserToCalenderGroup(int groupId, string userId);
        Task<bool> AddCalenderEvent(AddEventViewModel model);
    }
}
