using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("BOOKED_TRIP")]
    public partial class BookedTrip
    {
        [Key]
        [Column("Trip_Id")]
        public int TripId { get; set; }
        [Required]
        [Column("Client_Name")]
        [StringLength(30)]
        public string ClientName { get; set; }
        [Column("Party_Size")]
        public int PartySize { get; set; }
        [Column("Target_Fish")]
        [StringLength(20)]
        public string TargetFish { get; set; }
        public int Region { get; set; }
        public int? Location { get; set; }
        [Column("Fishing_Style")]
        [StringLength(30)]
        public string FishingStyle { get; set; }
        [Column("Guide_Name")]
        [StringLength(30)]
        public string GuideName { get; set; }
        [Column("Trip_End", TypeName = "date")]
        public DateTime TripEnd { get; set; }
    }
}
