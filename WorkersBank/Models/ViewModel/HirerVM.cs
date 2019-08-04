using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class HirerVM
    {
        public HirerVM()
        {
                
        }
        public HirerVM(HirerDTO row)
        {
            Id = row.Id;
            UserId = row.UserId;
            NameOfOrganisation = row.NameOfOrganisation;
            Address = row.Address;
            Description = row.Description;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string NameOfOrganisation { get; set; }
        [AllowHtml]
        public string Address { get; set; }
        [AllowHtml]
        public string Description { get; set; }
    }
}