using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkersBank.Models.ViewModel
{
    public class SearchVM
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Qualification { get; set; }
        public int Job { get; set; }
        public string Gender { get; set; }


        public IEnumerable<SelectListItem> Jobs { get; set; }
    }
}