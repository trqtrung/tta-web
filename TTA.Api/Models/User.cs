using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("users")]
    public class User : EntityBase
    {
        [Required]
        [Column("username")]
        public string Username { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("active")]
        public Boolean Active { get; set; }

        [Column("hash")]
        public byte[] PasswordHash { get; set; }

        [Column("salt")]
        public byte[] PasswordSalt { get; set; }
    }
}
