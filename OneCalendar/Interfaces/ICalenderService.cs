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

        Task<List<ShortHandCalanderGroup>> GetCalenderGroups();
        IEnumerable<GroupResponse> GetGroupsAndEvents(Guid userId);
        Task<CalenderGroup> AddUserToCalenderGroup(int groupId, string userId);
        Task<bool> AddCalenderEvent(AddEventViewModel model);
        Task<bool> DeleteCalenderEvent(DeleteEventViewModel model);
        Task<bool> UpdateCalenderEvent(UpdateEventViewModel model);
        Task<bool> GroupExistById(int groupId);
        Task<CalenderGroup> RemoveUserFromCalenderGroup(int groupId, string userId);
        Task<bool> UserExistsInGroup(int groupId, string userId);
        Task<bool> AddGroup(AddCalenderGroupViewModel model);
        Task<bool> GroupExistsByName(string groupName);
        Task<bool> DeleteGroupsById(List<int> groups);
        Task<bool> GroupsExistsById(List<int> groups);
    }
}
