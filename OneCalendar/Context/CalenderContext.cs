using Microsoft.EntityFrameworkCore;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Context
{
    public class CalenderContext : DbContext
    {
        public CalenderContext(DbContextOptions<CalenderContext> options) : base(options)
        {
        }

        public DbSet<CalenderTask> CalenderTasks { get; set; }
        public List<CalenderGroup> CalenderGroups { get; set; }
    }
}



//dotnet ef migrations add <migrationName> --context <contextName>
//dotnet ef database update --context <contextName>

//To revert to previous migration
//dotnet ef database update <migrationName> --context <contextName>

