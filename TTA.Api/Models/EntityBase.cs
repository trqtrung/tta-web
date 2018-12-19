using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    public abstract class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("created")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset Created { get; set; }

        [Column("updated")]
        public DateTimeOffset Updated { get; set; }
    }
}
