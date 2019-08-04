using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class ChangePassport
    {
        [Required(ErrorMessage = "Please upload your passport")]
        public HttpPostedFileBase Passport { get; set; }
    }
}