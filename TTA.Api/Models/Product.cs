using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("products")]
    public class Product
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("product_code")]
        public string ProductCode { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("type")]
        public int? Type { get; set; }

        [Column("brand_id")]
        public int? BrandID { get; set; }

        [Column("sku")]
        public string SKU { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("enabled")]
        public bool? Enabled { get; set; }

        [Column("size_inch")]
        public decimal? SizeInch { get; set; }

        [Column("size_mm")]
        public decimal? SizeMM { get; set; }

        [Column("weight")]
        public decimal? Weight { get; set; }

        [Column("height")]
        public decimal? Height { get; set; }

        [Column("width")]
        public decimal? Width { get; set; }

        [Column("length")]
        public decimal? Length { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<BuyingPrice> BuyingPrices { get; set; }

        public virtual ICollection<SellingPrice> SellingPrices { get; set; }

        public virtual ICollection<ProductDescription> ProductDescriptions { get; set; }

        public virtual Brand Brands { get; set; }

        [NotMapped]
        public double Price { get; set; }

        [NotMapped]
        public double BuyingPrice { get; set; }

        [NotMapped]
        public int? SupplierID { get; set; }
    }
}
