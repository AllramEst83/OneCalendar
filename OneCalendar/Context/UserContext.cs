using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Data
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}

     //Only migrate with this powerShell command (the other contexts will be auto generated)
        //dotnet ef database update --context UserContext
        //dotnet ef migrations add --> namn på migrationen: start
        //dotnet ef database update
    
