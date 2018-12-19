using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("customers")]
    public class Customer : EntityBase
    {
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("last_login")]
        public DateTime LastLogin {get;set;}
    }
}
