using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("SEASON")]
    public partial class Season
    {
        [Key]
        [Column("Season_ID")]
        public int SeasonId { get; set; }
        [Required]
        [Column("Season_name")]
        [StringLength(30)]
        public string SeasonName { get; set; }
    }
}
