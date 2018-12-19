using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTA.Api.Models
{
    [Table("option_lists")]
    public class OptionList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("key")]
        public string Key { get; set; }
    
        [Column("value")]
        public string Value { get; set; }
        
        [Column("record_guid")]
        public Guid? RecordGuid { get; set; }
    }
}
