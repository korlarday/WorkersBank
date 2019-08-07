using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Areas.Admin.Models
{
    public class ManageUsersVM
    {
        public List<UsersVM> Administrators { get; set; }
        public List<UsersVM> Nonadministrators { get; set; }
    }
}