using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersBank.Areas.Admin.Models
{
    public class SetAuthorization
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool Admin { get; set; }
    }
}