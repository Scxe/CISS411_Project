using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class SwimDbContext : IdentityDbContext<RegisteredUser>
    {
        public SwimDbContext(DbContextOptions<SwimDbContext> options) : base(options)
        { }
    }
}
