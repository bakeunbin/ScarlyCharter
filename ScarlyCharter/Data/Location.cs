using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("LOCATION_")]
    public partial class Location
    {
        [Key]
        [Column("Location_ID")]
        public int LocationId { get; set; }
        [Column("Region_ID")]
        public int RegionId { get; set; }
        [Required]
        [StringLength(30)]
        public string Type { get; set; }
        public bool? Availability { get; set; }

        [ForeignKey(nameof(RegionId))]
        [InverseProperty("Locations")]
        public virtual Region Region { get; set; }
    }
}
