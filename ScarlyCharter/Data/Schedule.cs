using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("SCHEDULE")]
    public partial class Schedule
    {
        public Schedule()
        {
            BookedTrips = new HashSet<BookedTrip>();
        }

        [Key]
        [Column("Schedule_ID")]
        public int ScheduleId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Column("Start_Time")]
        public TimeSpan StartTime { get; set; }
        [Column("End_Time")]
        public TimeSpan EndTime { get; set; }

        [InverseProperty(nameof(BookedTrip.Schedule))]
        public virtual ICollection<BookedTrip> BookedTrips { get; set; }
    }
}
