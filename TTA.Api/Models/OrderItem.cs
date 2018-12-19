using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("order_items")]
    public class OrderItem
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("discount")]
        public decimal Discount { get; set; }

        [Column("order_id")]
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

    }
}
