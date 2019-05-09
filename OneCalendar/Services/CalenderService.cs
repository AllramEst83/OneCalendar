using Microsoft.EntityFrameworkCore;
using OneCalendar.Context;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.ResponseModels;
using OneCalendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Services
{
    public class CalenderService : ICalenderService
    {
        public CalenderService(CalenderContext calenderContext, IAccountService accountService)
        {
            CalenderContext = calenderContext;
            AccountService = accountService;
        }

        public CalenderContext CalenderContext { get; }
        public IAccountService AccountService { get; }

        public async Task<bool> DeleteCalenderEvent(DeleteEventViewModel model)
        {
            if (await EventExists(model.EventId))
            {
                CalenderTask calenderTaskToDelete = await CalenderContext.CalenderTasks.FirstOrDefaultAsync(x => x.Id == model.EventId);
                CalenderGroup calenderGroup = await CalenderContext.CalenderGroups.Include(i => i.CalenderTasks).FirstOrDefaultAsync(x => x.Id == model.GroupId);

                IEnumerable<CalenderTask> filteredCalenderTasks = calenderGroup.CalenderTasks.Where(x => x.Id != model.EventId);
                calenderGroup.CalenderTasks = filteredCalenderTasks.ToList();

                CalenderContext.CalenderGroups.Update(calenderGroup);
                CalenderContext.CalenderTasks.Remove(calenderTaskToDelete);
                SaveChanges();

                return true;
            }

            return false;
        }

        private async Task<bool> EventExists(int? eventId)
        {
            CalenderTask eventExists = await CalenderContext.CalenderTasks.FirstOrDefaultAsync(x => x.Id == eventId);
            return eventExists == null ? false : true;
        }


        public async Task<bool> UpdateCalenderEvent(UpdateEventViewModel model)
        {
            User user = await AccountService.GetUserByEmail(model.UserName);
            if (await EventExists(model.EventId) && user != null)
            {
                DateTime startDate = DateTime.Parse(model.Start);
                DateTime endDate = DateTime.Parse(model.End);
                string userId = model.UserId.ToString();
                string description = model.Description.Trim();
                string title = model.Title.Trim();

                CalenderTask taskToUpdate = await CalenderContext.CalenderTasks.FirstOrDefaultAsync(t => t.Id == model.EventId);

                EditedByUser editedByUser = new EditedByUser()
                {
                    DateOfEdit = DateTime.Now,
                    EditedByUserId = user.Id
                };

                List<EditedByUser> editedByList;

                if (taskToUpdate.Edited == null)
                {
                    editedByList = new List<EditedByUser>();

                }
                else
                {
                    editedByList = taskToUpdate.Edited.ToList();

                }

                editedByList.Add(editedByUser);

                taskToUpdate.TaskName = title;
                taskToUpdate.TaskDescription = description;
                taskToUpdate.StartDate = startDate;
                taskToUpdate.EndDate = endDate;
                taskToUpdate.Edited = editedByList;

                CalenderContext.CalenderTasks.Update(taskToUpdate);
                SaveChanges();


                List<CalenderGroup> calenderGroups = CalenderContext.CalenderGroups.Include(i => i.CalenderTasks).ToList();
                CalenderGroup caldenrGroupToREMOVE_TaskFrom;

                foreach (CalenderGroup group in calenderGroups)
                {
                    foreach (CalenderTask task in group.CalenderTasks)
                    {
                        if (task.Id == model.EventId.Value)
                        {
                            if (group.Id != model.GroupId)
                            {
                                caldenrGroupToREMOVE_TaskFrom = group;
                                caldenrGroupToREMOVE_TaskFrom.CalenderTasks.Remove(task);

                                CalenderGroup newGroupToAddToTaskTo = await CalenderContext.CalenderGroups.FirstOrDefaultAsync(x => x.Id == model.GroupId);
                                newGroupToAddToTaskTo.CalenderTasks.Add(task);

                                CalenderContext.CalenderGroups.Update(caldenrGroupToREMOVE_TaskFrom);
                                CalenderContext.CalenderGroups.Update(newGroupToAddToTaskTo);
                                SaveChanges();
                                break;
                            }

                        }
                    }
                }
                return true;
            }

            return false;
        }

        public async Task<bool> AddCalenderEvent(AddEventViewModel model)
        {
            bool userExists = await AccountService.UserExistByUserName(model.UserName);
            if (userExists)
            {
                DateTime startDate = DateTime.Parse(model.Start);
                DateTime endDate = DateTime.Parse(model.End);
                string userId = model.UserId.ToString();
                CalenderTask calenderTask = new CalenderTask()
                {
                    TaskName = model.Title.Trim(),
                    StartDate = startDate,
                    EndDate = endDate,
                    TaskDescription = model.Description.Trim(),
                    CreatedBy = userId,
                    Edited = null
                };
                await CalenderContext.CalenderTasks.AddAsync(calenderTask);
                SaveChanges();

                CalenderGroup calenderGroup = await CalenderContext.CalenderGroups.Include(i => i.CalenderTasks).FirstOrDefaultAsync(x => x.Id == model.GroupId);

                List<string> listOfUserIds = calenderGroup.ListOfUserIds.ToList();
                List<CalenderTask> listOfTasks = calenderGroup.CalenderTasks;

                CheckIfUserIsInList(listOfUserIds, userId);
                listOfTasks.Add(calenderTask);

                calenderGroup.CalenderTasks = listOfTasks;
                calenderGroup.ListOfUserIds = listOfUserIds.ToArray();

                CalenderContext.CalenderGroups.Update(calenderGroup);
                SaveChanges();

                return true;

            }

            return false;
        }

        private void CheckIfUserIsInList(List<string> list, string userId)
        {
            if (!list.Contains(userId))
            {
                list.Add(userId);
            }
        }

        public async Task<List<ShortHandCalanderGroup>> GetCalenderGroups()
        {
            List<CalenderGroup> groups = await Task.FromResult(CalenderContext.CalenderGroups.ToList());
            List<ShortHandCalanderGroup> shorthandGroupsList = groups.Select(x => new ShortHandCalanderGroup() { Id = x.Id, Name = x.Name }).OrderBy(o => o.Name).ToList();

            return shorthandGroupsList;
        }

        public async Task<bool> GroupExistById(int groupId)
        {
            bool groupExist = await CalenderContext.CalenderGroups.AnyAsync(x => x.Id == groupId);

            return groupExist;
        }

        public async Task<CalenderGroup> AddUserToCalenderGroup(int groupId, string userId)
        {
            CalenderGroup calenderGroup = await CalenderContext.CalenderGroups.FirstOrDefaultAsync(x => x.Id == groupId);

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

        public async Task<bool> UserExistsInGroup(int groupId, string userId)
        {
            bool result = false;
            CalenderGroup group;
            if (!string.IsNullOrEmpty(userId))
            {
                group = await CalenderContext.CalenderGroups.FirstOrDefaultAsync(x=>x.Id == groupId);
                if (group != null)
                {
                    result = group.ListOfUserIds.Contains(userId);
                }
            }

            return result;
        }

        public async Task<CalenderGroup> RemoveUserFromCalenderGroup(int groupId, string userId)
        {
            CalenderGroup calenderGroup = await CalenderContext.CalenderGroups.FirstOrDefaultAsync(x => x.Id == groupId);

            if (calenderGroup == null)
            {
                return null;
            }

            List<string> listOfIds = calenderGroup.ListOfUserIds.ToList();
            string user = listOfIds.SingleOrDefault(i => i == userId);
            if (user != null)
            {
                listOfIds.Remove(user);
            }

            calenderGroup.ListOfUserIds = listOfIds.ToArray();

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
                    Description = p.TaskDescription,
                    End = p.EndDate,
                    Color = p.Color,
                    TextColor = p.TextColor,
                    AllDay = false

                }).ToList()

            });

            return groupsAndEvents;

        }
    }
}
