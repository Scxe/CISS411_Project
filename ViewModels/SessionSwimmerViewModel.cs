using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CISS411_Project.Models;

namespace CISS411_Project.ViewModels
{
    public class SessionSwimmerViewModel
    {
        public IEnumerable<Session> Sessions { get; set; }

        public Swimmer Swimmer { get; set; } 
    }
}
