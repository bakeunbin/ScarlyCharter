using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("REGION")]
    public partial class Region
    {
        public Region()
        {
            Fish = new HashSet<Fish>();
            Guides = new HashSet<Guide>();
            Locations = new HashSet<Location>();
        }

        [Key]
        [Column("Region_ID")]
        public int RegionId { get; set; }
        [Required]
        [Column("Region_Name")]
        [StringLength(30)]
        public string RegionName { get; set; }
        [Column("Season_ID")]
        public int? SeasonId { get; set; }

        [ForeignKey(nameof(SeasonId))]
        [InverseProperty("Regions")]
        public virtual Season Season { get; set; }
        [InverseProperty("Region")]
        public virtual ICollection<Fish> Fish { get; set; }
        [InverseProperty(nameof(Guide.Region))]
        public virtual ICollection<Guide> Guides { get; set; }
        [InverseProperty(nameof(Location.Region))]
        public virtual ICollection<Location> Locations { get; set; }
    }
}
