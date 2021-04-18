using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("GUIDE")]
    public partial class Guide
    {
        public Guide()
        {
            BookedTrips = new HashSet<BookedTrip>();
        }

        [Key]
        [Column("Guide_ID")]
        public int GuideId { get; set; }
        [Required]
        [Column("Guide_Name")]
        [StringLength(30)]
        public string GuideName { get; set; }
        [Column("Region_ID")]
        public int RegionId { get; set; }
        [Column("Season_ID")]
        public int? SeasonId { get; set; }
        [Column("Fishing_Style")]
        [StringLength(30)]
        public string FishingStyle { get; set; }
        public bool? Overnight { get; set; }

        [ForeignKey(nameof(RegionId))]
        [InverseProperty("Guides")]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(SeasonId))]
        [InverseProperty("Guides")]
        public virtual Season Season { get; set; }
        [InverseProperty(nameof(BookedTrip.Guide))]
        public virtual ICollection<BookedTrip> BookedTrips { get; set; }
    }
}
