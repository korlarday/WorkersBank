using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkersBank.Models.ViewModel
{
    public class SearchVM
    {
        public string Location { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Qualification { get; set; }
        public int Job { get; set; }

        public IEnumerable<SelectListItem> Jobs { get; set; }
    }
}