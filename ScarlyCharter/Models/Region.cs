using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("REGION")]
    public partial class Region
    {
        public Region()
        {
            Locations = new HashSet<Location>();
        }

        [Key]
        [Column("Region_ID")]
        public int RegionId { get; set; }
        [Required]
        [Column("Region_Name")]
        [StringLength(30)]
        public string RegionName { get; set; }
        public int? Location { get; set; }
        [StringLength(30)]
        public string Fish { get; set; }
        public int? Season { get; set; }

        [InverseProperty("Region")]
        public virtual ICollection<Location> Locations { get; set; }
    }
}
