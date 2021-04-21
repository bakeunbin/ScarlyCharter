using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace ScarlyCharter.Data
{
    [Table("CLIENT")]
    public partial class Client
    {
        public Client()
        {
            BookedTrips = new HashSet<BookedTrip>();
        }

        [Key]
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Column("Client_Name")]
        [StringLength(30)]
        public string ClientName { get; set; }
        [Column("Payment_Info")]
        [StringLength(30)]
        public string PaymentInfo { get; set; }
        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        [StringLength(256)]
        public string Password { get; set; }

        [InverseProperty(nameof(BookedTrip.Client))]
        public virtual ICollection<BookedTrip> BookedTrips { get; set; }
    }
}
