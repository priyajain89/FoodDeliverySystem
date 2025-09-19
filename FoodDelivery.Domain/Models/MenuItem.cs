using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

public partial class MenuItem
{
    [Key]
    public int ItemId { get; set; }

    public int? RestaurantId { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    public bool? IsAvailable { get; set; }
    public string Category { get; set; } = string.Empty;

    public string? FoodImage { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Item")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("RestaurantId")]
    [InverseProperty("MenuItems")]
    public virtual Restaurant? Restaurant { get; set; }
}
