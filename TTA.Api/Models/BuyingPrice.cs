using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("buying_prices")]
    public class BuyingPrice
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        //public virtual Supplier Supplier { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Column("price_date")]
        public DateTime PriceDate { get; set; }

        [Column("price")]
        public double Price { get; set; }
    }
}
