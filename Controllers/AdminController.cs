// AT 10-15-20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CISS411_Project.Models;
using CISS411_Project.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CISS411_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SwimDbContext db;
        public AdminController(SwimDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        // Add Lessons
        public IActionResult AddLesson()
        {
            Lesson lesson = new Lesson();
            return View(lesson);
        }
        [HttpPost]
        public async Task<IActionResult> AddLesson(Lesson lesson)
        {
            db.Add(lesson);
            await db.SaveChangesAsync();
            return RedirectToAction("AllLesson", "Admin");
        }
        // View Lessons
        public IActionResult AllLesson()
        {
            var lesson = db.Lessons.ToList();
            return View(lesson);
        }
    }
}
