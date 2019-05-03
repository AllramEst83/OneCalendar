using Microsoft.EntityFrameworkCore;
using OneCalendar.Context;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Services
{
    public class CalenderService : ICalenderService
    {
        public CalenderService(CalenderContext calenderContext)
        {
            CalenderContext = calenderContext;
        }

        public CalenderContext CalenderContext { get; }

        public List<ShortHandCalanderGroup> GetCalenderGroups()
        {
            List<ShortHandCalanderGroup> groups =
               CalenderContext
                .CalenderGroups.Select(x => new ShortHandCalanderGroup() { Id = x.Id, Name = x.Name }).ToList();
            return groups;
        }

        public CalenderGroup AddUserToCalenderGroup(int groupId, string userId)
        {
            CalenderGroup calenderGroup = CalenderContext.CalenderGroups.FirstOrDefault(x => x.Id == groupId);

            if (calenderGroup == null)
            {
                return null;
            }

            List<string> listOfUserIds = calenderGroup.ListOfUserIds.ToList();

            listOfUserIds.Add(userId);

            calenderGroup.ListOfUserIds = listOfUserIds.ToArray();

            CalenderContext.CalenderGroups.Update(calenderGroup);

            SaveChanges();

            return calenderGroup;
        }

        private void SaveChanges()
        {
            CalenderContext.SaveChanges();

        }

        public IEnumerable<GroupResponse> GetGroupsAndEvents(Guid userId)
        {
            List<int> groupIds = new List<int>();

            //Get all calenderGroups Without tasks
            List<CalenderGroup> allGroups =
                  CalenderContext.CalenderGroups.ToList();

            //Check if user id exists in any usergroup
            foreach (CalenderGroup item in allGroups)
            {
                int position = Array.IndexOf(item.ListOfUserIds, userId.ToString());
                if (position > -1)
                {
                    //Add groupId when userId is found in that group
                    groupIds.Add(item.Id);
                }
            }

            //Get all groups AND tasks
            List<CalenderGroup> userGroups = CalenderContext
                .CalenderGroups.Where(x =>
                groupIds
                .Contains(x.Id))
                .Include(i => i.CalenderTasks).ToList();

            //Get all tasks and selected group properties
            IEnumerable<GroupResponse> groupsAndEvents = userGroups.Select(x =>
            new GroupResponse()
            {
                GroupId = x.Id,
                GroupName = x.Name,
                Events = x.CalenderTasks.Select(p =>
                new TaskResponse()
                {
                    Id = p.Id,
                    Title = p.TaskName,
                    Start = p.StartDate,
                    End = p.EndDate,
                    AllDay = false

                }).ToList()

            });

            return groupsAndEvents;

        }
    }
}
