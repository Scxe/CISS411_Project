﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class Swimmer
    {
        public int SwimmerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public char Gender { get; set; }
        public string DateOfBirth { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
