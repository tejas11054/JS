using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TimeSheet.Models;

namespace TimeSheet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasData(new UserRole { RoleID = 1, Role = Role.ADMIN },
                new UserRole { RoleID = 2, Role = Role.EMPLOYEE });
        }
    }
}
