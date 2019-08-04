using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblUserRoles")]
    public class UserRolesDTO
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual UsersDTO User { get; set; }

        [ForeignKey("RoleId")]
        public virtual RolesDTO Role { get; set; }
    }
}