using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("BOOKED_TRIP")]
    public partial class BookedTrip
    {
        [Key]
        [Column("Trip_ID")]
        public int TripId { get; set; }
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Column("Schedule_ID")]
        public int ScheduleId { get; set; }
        [Column("Location_ID")]
        public int LocationId { get; set; }
        [Column("Guide_ID")]
        public int GuideId { get; set; }
        [Column("Target_Fish_ID")]
        public int? TargetFishId { get; set; }
        [Column("Party_Size")]
        public int PartySize { get; set; }
        [Column("Fishing_Style")]
        [StringLength(30)]
        public string FishingStyle { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("BookedTrips")]
        public virtual Client Client { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        [InverseProperty("BookedTrips")]
        public virtual Schedule Schedule { get; set; }
    }
}
