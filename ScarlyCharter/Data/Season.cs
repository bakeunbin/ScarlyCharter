using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("SEASON")]
    public partial class Season
    {
        public Season()
        {
            Fish = new HashSet<Fish>();
            Guides = new HashSet<Guide>();
            Regions = new HashSet<Region>();
        }

        [Key]
        [Column("Season_ID")]
        public int SeasonId { get; set; }
        [Required]
        [Column("Season_name")]
        [StringLength(30)]
        public string SeasonName { get; set; }

        [InverseProperty("Season")]
        public virtual ICollection<Fish> Fish { get; set; }
        [InverseProperty(nameof(Guide.Season))]
        public virtual ICollection<Guide> Guides { get; set; }
        [InverseProperty(nameof(Region.Season))]
        public virtual ICollection<Region> Regions { get; set; }
    }
}
