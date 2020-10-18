using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.Models
{
    public class Coach
    {
        public int CoachId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public virtual RegisteredUser User { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}
