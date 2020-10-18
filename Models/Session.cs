using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SeatsAvailable { get; set; }
        public string StartTime { get; set; }
        public int LessonId { get; set; }
        public int CoachId { get; set; }
        public Lesson Lesson { get; set; }
        public Coach Coach { get; set; }
        public string ProgressReport { get; set; }
    }
}
