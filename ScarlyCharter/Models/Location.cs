using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("LOCATION")]
    public partial class Location
    {
        public Location()
        {
            Fish = new HashSet<Fish>();
        }

        [Key]
        [Column("Location_ID")]
        public int LocationId { get; set; }
        [Column("Region_ID")]
        public int RegionId { get; set; }
        public bool? River { get; set; }
        public bool? Lake { get; set; }
        public bool? Inshore { get; set; }
        public bool? Offshore { get; set; }
        public bool? Availability { get; set; }

        [ForeignKey(nameof(RegionId))]
        [InverseProperty("Locations")]
        public virtual Region Region { get; set; }
        [InverseProperty("LocationNavigation")]
        public virtual ICollection<Fish> Fish { get; set; }
    }
}
