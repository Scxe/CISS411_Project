using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class SwimDbContext : IdentityDbContext<RegisteredUser>
    {
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Swimmer> Swimmers { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public SwimDbContext(DbContextOptions<SwimDbContext> options) : base(options)
        { }
    }
}
