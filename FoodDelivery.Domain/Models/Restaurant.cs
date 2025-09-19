using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Domain.Models;

public partial class Restaurant
{
    [Key]
    public int RestaurantId { get; set; }

    public int? UserId { get; set; }

    public string? Address { get; set; }

    [StringLength(50)]
    public string? FssaiId { get; set; }

    public int? PinCode { get; set; }

    public string? FssaiImage { get; set; }

    public string? TradelicenseImage { get; set; }

    [StringLength(50)]
    public string? TradeId { get; set; }

    [StringLength(50)]
    public string? Latitude { get; set; }

    [StringLength(50)]
    public string? Longitude { get; set; }

    [JsonIgnore]

    [InverseProperty("Restaurant")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [JsonIgnore]
    [InverseProperty("Restaurant")]
    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    [JsonIgnore]
    [InverseProperty("Restaurant")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [JsonIgnore]
    [ForeignKey("UserId")]
    [InverseProperty("Restaurants")]
    public virtual User? User { get; set; }
}
