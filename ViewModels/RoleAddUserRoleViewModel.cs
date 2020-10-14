// AT 10-14-20
using CISS411_Project.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CISS411_Project.ViewModels
{
    public class RoleAddUserRoleViewModel
    {
        public RegisteredUser User { get; set; }
        public string Role { get; set; }
        public SelectList RoleList { get; set; }
    }
}
