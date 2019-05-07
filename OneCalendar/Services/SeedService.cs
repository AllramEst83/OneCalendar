using Microsoft.EntityFrameworkCore;
using OneCalendar.Context;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Services
{
    public class SeedService : ISeedService
    {
        public SeedService(CalenderContext calenderContext, IAccountService accountService)
        {
            _calenderContext = calenderContext;
            _accountService = accountService;
        }

        public CalenderContext _calenderContext { get; }
        public IAccountService _accountService { get; }

        public void SeedCalenderTasks()
        {
            _calenderContext.Database.EnsureCreated();

            _calenderContext.EditedByUser.RemoveRange(_calenderContext.EditedByUser);
            _calenderContext.CalenderTasks.RemoveRange(_calenderContext.CalenderTasks);
            _calenderContext.CalenderGroups.RemoveRange(_calenderContext.CalenderGroups);
            _calenderContext.SaveChanges();

            User user = _accountService.GetUserByEmail("Kajan@altavisat.com").Result;
            string Kay = user.Id;
            string Rebecka = "d0b16a9c-1fb0-440b-94b6-44fad5ed68b2";
            int Year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;


            List<CalenderTask> KaylistOfCalenderTasks = new List<CalenderTask>()
            {
                new CalenderTask()
                {
                    TaskName = "Seminarium",
                    StartDate = new DateTime(Year,month,DateTime.Now.Day,10,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.Day,15,00,00,DateTimeKind.Local),
                    CreatedBy = Kay,
                    TaskDescription = "Ett seminarium om saker och ting",
                    Edited = null,
                },
                new CalenderTask()
                {
                    TaskName = "Häsloundersökning",
                    StartDate = new DateTime(Year,month,DateTime.Now.AddDays(1).Day,10,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.AddDays(1).Day,15,00,00,DateTimeKind.Local),
                    CreatedBy = Kay,
                    TaskDescription = "PÅ vårdcentralen, var inte sen.",
                    Edited = null,

                },
                new CalenderTask()
                {
                    TaskName = "Handla grönsaker till festen",
                    StartDate = new DateTime(Year,month,DateTime.Now.AddDays(2).Day,10,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.AddDays(2).Day,15,00,00,DateTimeKind.Local),
                    CreatedBy = Kay,
                    TaskDescription = "Glöm inte papaya",
                    Edited = null,
                },

            };




            List<CalenderTask> RebeckaslistOfCalenderTasks = new List<CalenderTask>()
            {
                                new CalenderTask()
                {
                    TaskName = "Öppethus på skolan",
                    StartDate = new DateTime(Year,month,DateTime.Now.AddDays(3).Day,10,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.AddDays(3).Day,15,00,00,DateTimeKind.Local),
                    CreatedBy = Kay,
                    TaskDescription = "Glöm inte",
                    Edited = null,
                }
            };

            _calenderContext.CalenderTasks.AddRange(KaylistOfCalenderTasks);
            _calenderContext.CalenderTasks.AddRange(RebeckaslistOfCalenderTasks);
            _calenderContext.SaveChanges();


            string[] kaylistOfUserIds = new string[]
            {
                Kay,
            };
            string[] RebeckaslistOfUserIds = new string[]
    {
                Kay
    };

            List<CalenderGroup> listOfCalenderGroups = new List<CalenderGroup>()
            {
                new CalenderGroup()
                {
                    ListOfUserIds = kaylistOfUserIds,
                    CalenderTasks =   KaylistOfCalenderTasks,
                    Name ="Kays - Kalendergrupp"
                },
                   new CalenderGroup()
                {
                    ListOfUserIds = RebeckaslistOfUserIds,
                    CalenderTasks =   RebeckaslistOfCalenderTasks,
                    Name ="Rebeckas - Kalendergrupp"
                }
            };

            _calenderContext.CalenderGroups.AddRange(listOfCalenderGroups);
            _calenderContext.SaveChanges();
        }
    }
}
