﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class Swimmer
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public char Gender { get; set; }
        public string DateOfBirth { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}