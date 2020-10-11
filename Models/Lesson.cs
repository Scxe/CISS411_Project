using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class Lesson
    {
        public string SkillLevel { get; set; }
        public decimal Tuition { get; set; }
        public ICollection<Session> Sessions { get; set; }
}
}
