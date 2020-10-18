// AT 10-17-20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CISS411_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CISS411_Project.Controllers
{
    [Authorize(Roles ="Coach")]
    public class CoachController : Controller
    {
        private readonly SwimDbContext db;
        public CoachController(SwimDbContext db)
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
            Coach coach = new Coach();
            if(db.Coaches.Any(c => c.UserId == currentUserId))
            {
                coach = db.Coaches.FirstOrDefault(c => c.UserId == currentUserId);
            }
            else
            {
                coach.UserId = currentUserId;
            }
            return View(coach);
        }
        [HttpPost]
        public async Task<IActionResult> AddProfile(Coach coach)
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (db.Coaches.Any(c => c.UserId == currentUserId))
            {
                var coachToUpdate = db.Coaches.FirstOrDefault(c => c.UserId == currentUserId);
                coachToUpdate.Name = coach.Name;
                coachToUpdate.PhoneNumber = coach.PhoneNumber;
                db.Update(coachToUpdate);
            }
            else
            {
                db.Add(coach);
            }
            await db.SaveChangesAsync();
            return View("Index");
        }
        // All Lessons (for coach)
        public IActionResult AllLesson()
        {
            return View(db.Lessons);
        }
        // Add a Session
        public IActionResult AddSession(int id)
        {
            Session session = new Session();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            session.CoachId = db.Coaches.SingleOrDefault(i => i.UserId == currentUserId).CoachId;
            session.LessonId = db.Lessons.SingleOrDefault(i => i.LessonId == id).LessonId;
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> AddSession(Session session)
        {
            db.Add(session);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Coach");
        }
        //public async Task<IActionResult> SessionByCoach()
        //{
        //    var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var CoachId = db.Coaches.SingleOrDefault
        //        (i => i.UserId == currentUserId).CoachId;
        //    var session = await db.Sessions.Where(i => i.CoachId == CoachId).ToListAsync();
        //    return View(session);
        //}
        public async Task<IActionResult> PostReport(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var allSwimmers = await db.Enrollments.Include(c => c.Session).Where(c => c.SessionId == id).ToListAsync();
            if (allSwimmers == null)
            {
                return NotFound();
            }
            return View(allSwimmers);
        }
        [HttpPost]
        public IActionResult PostReport(List<Enrollment> enrollments)
        {
            foreach (var enrollment in enrollments)
            {
                var er = db.Enrollments.Find(enrollment.EnrollmentId);
                er.ProgressReport = enrollment.ProgressReport;
            }

            db.SaveChanges();
            return RedirectToAction("SessionByCoach");
        }
    }
}
