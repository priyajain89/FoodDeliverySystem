using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Domain.Models;

[Index("Phone", Name = "UQ__Users__5C7E359E65C0615A", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D105344F15E0C6", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    public bool? IsVerified { get; set; }

    [StringLength(20)]
    public string? Role { get; set; }

    [InverseProperty("User")]
    [JsonIgnore]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<DeliveryAgent> DeliveryAgents { get; set; } = new List<DeliveryAgent>();

    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [JsonIgnore]
    [InverseProperty("User")]
    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
