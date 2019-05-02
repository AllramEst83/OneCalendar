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
        public SeedService(CalenderContext calenderContext)
        {
            _calenderContext = calenderContext;
        }

        public CalenderContext _calenderContext { get; }

        public void SeedCalenderTasks()
        {
            _calenderContext.Database.EnsureCreated();
            _calenderContext.CalenderTasks.RemoveRange(_calenderContext.CalenderTasks);
            _calenderContext.CalenderGroups.RemoveRange(_calenderContext.CalenderGroups);

            string Kay = "64b2fdbf-1280-4cdf-ac3d-a13b9ab14e72";
            string Rebecka = "d0b16a9c-1fb0-440b-94b6-44fad5ed68b2";
            int Year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;


            List<CalenderTask> KaylistOfCalenderTasks = new List<CalenderTask>()
            {
                new CalenderTask()
                {
                    TaskName = "Seminarium",
                    StartDate = new DateTime(Year,month,day,13,15,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,day,18,30,00,DateTimeKind.Local),
                    CreatedBy = Kay,
                    TaskDescription = "Ett seminarium om saker och ting",
                    Edited = null,
                },
                new CalenderTask()
                {
                    TaskName = "Häsloundersökning",
                    StartDate = new DateTime(Year,month,DateTime.Now.AddDays(1).Day,17,45,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.AddDays(1).Day,18,30,00,DateTimeKind.Local),
                    CreatedBy = Kay,
                    TaskDescription = "PÅ vårdcentralen, var inte sen.",
                    Edited = null,

                },
                new CalenderTask()
                {
                    TaskName = "Handla grönsaker till festen",
                    StartDate = new DateTime(Year,month,DateTime.Now.AddDays(2).Day,09,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.AddDays(2).Day,10,30,00,DateTimeKind.Local),
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
                    StartDate = new DateTime(Year,month,DateTime.Now.AddDays(3).Day,18,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(Year,month,DateTime.Now.AddDays(3).Day,20,30,00,DateTimeKind.Local),
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
                    Name ="Kays kalendergrupp"
                },
                   new CalenderGroup()
                {
                    ListOfUserIds = RebeckaslistOfUserIds,
                    CalenderTasks =   RebeckaslistOfCalenderTasks,
                    Name ="Rebeckas Grupp"
                }
            };

            _calenderContext.CalenderGroups.AddRange(listOfCalenderGroups);
            _calenderContext.SaveChanges();
        }
    }
}
