using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TTA.Api.Models
{
    [Table("order_tracking")]
    public class OrderTracking
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Column("order_id")]
        public Guid OrderId { get; set; }
        
        [Column("type")]
        public string Type { get; set; }
        
        [Column("tracking_time")]
        public DateTime TrackingTime { get; set; }

        [Column("Action")]
        public int? Action { get; set; }
    }
}
