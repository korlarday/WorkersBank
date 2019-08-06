using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class MailsVM
    {
        public MailsVM()
        {

        }
        public MailsVM(MailsDTO row)
        {
            Id = row.Id;
            Subject = row.Subject;
            Message = row.Message;
            NameOfCompany = row.NameOfCompany;
            Address = row.Address;
            Description = row.Description;
            SentBy = row.SentBy;
            Recipient = row.Recipient;
            TimeSent = row.TimeSent;
            Sender = new UsersVM(row.Sender);
            Reciever = new UsersVM(row.Reciever);
        }

        public int Id { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        public string NameOfCompany { get; set; }
        [AllowHtml]
        public string Address { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int? SentBy { get; set; }
        public int? Recipient { get; set; }
        public DateTime? TimeSent { get; set; }
        public string Slug { get; set; }
        public string Username { get; set; }
        public bool SaveMessage { get; set; }

        public UsersVM Sender { get; set; }
        public UsersVM Reciever { get; set; }
    }
}