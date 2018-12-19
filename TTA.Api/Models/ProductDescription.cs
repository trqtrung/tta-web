using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("product_descriptions")]
    public class ProductDescription
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("language")]
        public string Language { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created")]
        public DateTime? Created { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
