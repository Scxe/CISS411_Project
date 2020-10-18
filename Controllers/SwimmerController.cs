// SH 10-18-20

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CISS411_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CISS411_Project.Controllers
{
    [Authorize(Roles = "Swimmer")]
    public class SwimmerController : Controller
    {
        private readonly SwimDbContext db;
        public SwimmerController(SwimDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Add Profile
        public IActionResult AddProfile()
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Swimmer swimmer = new Swimmer();
            if (db.Swimmers.Any(i => i.UserId == currentUserId))
            {
                swimmer = db.Swimmers.FirstOrDefault(i => i.UserId == currentUserId);
            }
            else
            {
                swimmer.UserId = currentUserId;
            }
            return View(swimmer);
        }
        // http action method
        [HttpPost]
        public async Task<IActionResult> AddProfile(Swimmer swimmer)
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (db.Swimmers.Any(i => i.UserId == currentUserId))
            {
                var swimmerToUpdate = db.Swimmers.FirstOrDefault(i => i.UserId == currentUserId);
                swimmerToUpdate.Name = swimmer.Name;
                db.Update(swimmerToUpdate);
            }
            else
            {
                db.Add(swimmer);
            }
            await db.SaveChangesAsync();
            return View("Index");
        }
        //AllCourse method
        public async Task<IActionResult> AllSession()
        {
            var session = await db.Sessions.Include(c => c.Coach).ToListAsync();
            return View(session);
        }
        //Enroll Session Action
        public async Task<IActionResult> EnrollSession(int id)
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var swimmerId = db.Swimmers.FirstOrDefault(s => s.UserId == currentUserId).SwimmerId;
            Enrollment enrollment = new Enrollment
            {
                SessionId = id,
                SwimmerId = swimmerId
            };
            db.Add(enrollment);
            var session = await db.Sessions.FindAsync(enrollment.SessionId);
            session.SeatsAvailable--;
            await db.SaveChangesAsync();
            return View("Index");
        }
       
    }
}
