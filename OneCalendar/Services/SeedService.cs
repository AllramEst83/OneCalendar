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


            List<CalenderTask> KaylistOfCalenderTasks = new List<CalenderTask>()
            {
                new CalenderTask()
                {
                    TaskName = "Seminarium",
                    StartDate = new DateTime(2019,04,25,13,15,00,DateTimeKind.Local),
                    EndDate = new DateTime(2019,04,25,18,30,00,DateTimeKind.Local),
                    CreatedBy = "4bb4c370-763f-44b3-a9e2-23b9133d9d17",
                    TaskDescription = "Ett seminarium om saker och ting",
                    Edited = null,
                },
                new CalenderTask()
                {
                    TaskName = "Häsloundersökning",
                    StartDate = new DateTime(2019,04,29,17,45,00,DateTimeKind.Local),
                    EndDate = new DateTime(2019,04,29,18,30,00,DateTimeKind.Local),
                    CreatedBy = "4bb4c370-763f-44b3-a9e2-23b9133d9d17",
                    TaskDescription = "PÅ vårdcentralen, var inte sen.",
                    Edited = null,

                },
                new CalenderTask()
                {
                    TaskName = "Handla grönsaker till festen",
                    StartDate = new DateTime(2019,04,22,09,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(2019,04,22,10,30,00,DateTimeKind.Local),
                    CreatedBy = "4bb4c370-763f-44b3-a9e2-23b9133d9d17",
                    TaskDescription = "Glöm inte papaya",
                    Edited = null,
                },

            };


            List<CalenderTask> RebeckaslistOfCalenderTasks = new List<CalenderTask>()
            {
                                new CalenderTask()
                {
                    TaskName = "Öppethus på skolan",
                    StartDate = new DateTime(2019,04,30,18,00,00,DateTimeKind.Local),
                    EndDate = new DateTime(2019,04,30,20,30,00,DateTimeKind.Local),
                    CreatedBy = "d0b16a9c-1fb0-440b-94b6-44fad5ed68b2",
                    TaskDescription = "Glöm inte",
                    Edited = null,
                }
            };

            _calenderContext.CalenderTasks.AddRange(KaylistOfCalenderTasks);
            _calenderContext.CalenderTasks.AddRange(RebeckaslistOfCalenderTasks);
            _calenderContext.SaveChanges();


            string[] kaylistOfUserIds = new string[]
            {
                "4bb4c370-763f-44b3-a9e2-23b9133d9d17",
            };
            string[] RebeckaslistOfUserIds = new string[]
    {
                "d0b16a9c-1fb0-440b-94b6-44fad5ed68b2"
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
