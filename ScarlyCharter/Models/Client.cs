using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ScarlyCharter.Models
{
    [Table("CLIENT")]
    public partial class Client
    {
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
    }
}
