using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("suppliers")]
    public class Supplier
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("created")]
        public DateTime? Created { get; set; }

        public ICollection<BuyingPrice> ProductPrices { get; set; }
    }
}
