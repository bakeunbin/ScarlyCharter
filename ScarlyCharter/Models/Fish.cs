using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("FISH")]
    public partial class Fish
    {
        [Key]
        [Column("Fish_Name")]
        [StringLength(20)]
        public string FishName { get; set; }
        public int? Location { get; set; }
        public int Region { get; set; }
        public int? Season { get; set; }

        [ForeignKey(nameof(Location))]
        [InverseProperty("Fish")]
        public virtual Location LocationNavigation { get; set; }
    }
}
