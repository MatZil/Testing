using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Models.Entities;

namespace Xplicity_Holidays.Models
{
    public class SystemContext : DbContext
    {
        public SystemContext(DbContextOptions<SystemContext> options)
            : base(options)
        { }
        public SystemContext()
        { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}
