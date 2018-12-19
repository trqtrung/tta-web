using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("brands")]
    public class Brand
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("website")]
        public string Website { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created")]
        public DateTime? Created { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace TTA.Api.Models
//{
//    [Table("brands")]
//    public class Brand
//    {
//        [Column("id")]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        [Column("name")]
//        public string Name { get; set; }

//        [Column("website")]
//        public string Website { get; set; }

//        [Column("email")]
//        public string Email { get; set; }

//        [Column("description")]
//        public string Description { get; set; }

//        [Column("created")]
//        public DateTime? Created { get; set; }

//        public ICollection<Product> Products { get; set; }
//    }
//}
