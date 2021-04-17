using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("SCHEDULE")]
    public partial class Schedule
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Required]
        [Column("Guide_Name")]
        [StringLength(30)]
        public string GuideName { get; set; }
        public bool? Afternoon { get; set; }
        public bool? Overnight { get; set; }
        public bool? Evening { get; set; }
        public bool? Morning { get; set; }

        [ForeignKey(nameof(GuideName))]
        [InverseProperty(nameof(Guide.Schedules))]
        public virtual Guide GuideNameNavigation { get; set; }
    }
}
