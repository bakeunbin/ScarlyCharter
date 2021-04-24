using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("FISH")]
    public partial class Fish
    {
        [Key]
        [Column("Fish_ID")]
        public int FishId { get; set; }
        [Required]
        [Column("Fish_Name")]
        [StringLength(30)]
        public string FishName { get; set; }
        [Column("Region_ID")]
        public int RegionId { get; set; }
        [Column("Season_ID")]
        public int? SeasonId { get; set; }

        [ForeignKey(nameof(RegionId))]
        [InverseProperty("Fish")]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(SeasonId))]
        [InverseProperty("Fish")]
        public virtual Season Season { get; set; }
    }
}
