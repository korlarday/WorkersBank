using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblMails")]
    public class MailsDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string NameOfCompany { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int? SentBy { get; set; }
        public int? Recipient { get; set; }
        public DateTime? TimeSent { get; set; }
        public string Type { get; set; }

        [ForeignKey("SentBy")]
        public virtual UsersDTO Sender { get; set; }

        [ForeignKey("Recipient")]
        public virtual UsersDTO Reciever { get; set; }

    }
}