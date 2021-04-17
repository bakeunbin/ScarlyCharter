using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Keyless]
    [Table("SEASON")]
    public partial class Season
    {
        [Column("Season_ID")]
        public int SeasonId { get; set; }
        [Required]
        [Column("Season_name")]
        [StringLength(30)]
        public string SeasonName { get; set; }
        [Column("Guide_Available")]
        [StringLength(30)]
        public string GuideAvailable { get; set; }
    }
}
