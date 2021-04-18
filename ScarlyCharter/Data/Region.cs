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
            Guides = new HashSet<Guide>();
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
        [InverseProperty(nameof(Guide.Region))]
        public virtual ICollection<Guide> Guides { get; set; }
    }
}
