﻿// SH 10-18-20
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CISS411_Project.Models;
using CISS411_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Converters;

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
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var swimmer = db.Swimmers.FirstOrDefault(s => s.UserId == currentUserId);
            var session = await db.Sessions.Include(s => s.Coach).ToListAsync();
            SessionSwimmerViewModel vm = new SessionSwimmerViewModel();
            vm.Swimmer = swimmer;
            vm.Sessions = session;
            return View(vm);
        }
        //Enroll Session Action
        public async Task<IActionResult> EnrollSession(int id)
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var swimmerId = db.Swimmers.FirstOrDefault(s => s.UserId == currentUserId).SwimmerId;
            var enrollments = db.Enrollments.Include(e => e.Session).Where(e => e.SwimmerId == swimmerId);
            var session = await db.Sessions.FindAsync(id); // db.Sessions.FirstOrDefault(s => s.SessionId == id);
            var conflict = false;
            var dateString = "";
            DateTime starttime;
            DateTime endtime;
            DateTime enrolltime;
            var totalminutes = 0;

            foreach (var e in enrollments)
            {
                dateString = e.Session.StartDate + " " + e.Session.StartTime;
                if (DateTime.TryParse(dateString, out starttime)) { }

                dateString = e.Session.EndDate + " " + e.Session.StartTime;
                if (DateTime.TryParse(dateString, out endtime)) { }

                dateString = session.StartDate + " " + session.StartTime;
                if (DateTime.TryParse(dateString, out enrolltime)) { }
                
                foreach(DateTime day in Days(starttime, endtime))
                {
                    if (day.Date == enrolltime.Date)
                    {
                        totalminutes = (day.Hour * 60) + day.Minute;
                        totalminutes -= (enrolltime.Hour * 60) + day.Minute;

                        if (totalminutes <= 0)
                        {
                            if (totalminutes > -30)
                            {
                                conflict = true;
                            }
                        } else if (totalminutes > 0)
                        {
                            if (totalminutes < 30)
                            {
                                conflict = true;
                            }
                        }
                    }
                }
                
            }

            if (conflict)
            {
                return View("ScheduleConflict");
            }
            

            Enrollment enrollment = new Enrollment
            {
                SessionId = id,
                SwimmerId = swimmerId
            };
            db.Add(enrollment);
            session.SeatsAvailable--;
            await db.SaveChangesAsync();
            return View("Index");
        }
        public async Task<IActionResult> CheckReport()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserId == null)
            {
                return NotFound();
            }
            var swimmer = await db.Swimmers.SingleOrDefaultAsync(s => s.UserId == currentUserId);
            var swimmerId = swimmer.SwimmerId;
            var allSessions = await db.Enrollments.Include(e => e.Session).Where(c => c.SwimmerId == swimmerId).ToListAsync();
            if (allSessions == null)
            {
                return NotFound();
            }
            ViewData["sname"] = swimmer.Name;
            return View(allSessions);
        }
        public IEnumerable<DateTime> Days(DateTime from, DateTime to)
        {
            for (var day = from; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
