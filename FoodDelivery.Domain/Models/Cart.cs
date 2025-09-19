using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

[Table("Cart")]
public partial class Cart
{
    [Key]
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? RestaurantId { get; set; }

    public int? CartItemId { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("RestaurantId")]
    [InverseProperty("Carts")]
    public virtual Restaurant? Restaurant { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User? User { get; set; }
}
