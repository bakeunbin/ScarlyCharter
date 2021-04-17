using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("GUIDE")]
    public partial class Guide
    {
        public Guide()
        {
            Schedules = new HashSet<Schedule>();
        }

        [Key]
        [Column("Guide_Name")]
        [StringLength(30)]
        public string GuideName { get; set; }
        public int? Region { get; set; }
        public int? Location { get; set; }
        [Column("Fishing_Style")]
        [StringLength(30)]
        public string FishingStyle { get; set; }
        public bool? Overnight { get; set; }
        public int Season { get; set; }

        [InverseProperty(nameof(Schedule.GuideNameNavigation))]
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
