using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

public partial class Address
{
    [Key]
    public int AddressId { get; set; }

    public int? UserId { get; set; }

    [StringLength(255)]
    public string? AddressLine1 { get; set; }

    [StringLength(255)]
    public string? AddressLine2 { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(10)]
    public string? PinCode { get; set; }

    [StringLength(255)]
    public string? Landmark { get; set; }

    public bool? IsDefault { get; set; }

    [StringLength(50)]
    public string? Latitude { get; set; }

    [StringLength(50)]
    public string? Longitude { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("UserId")]
    [InverseProperty("Addresses")]
    public virtual User? User { get; set; }
}
