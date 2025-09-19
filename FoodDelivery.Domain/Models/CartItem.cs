using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

public partial class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart? Cart { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("CartItems")]
    public virtual MenuItem? Item { get; set; }
}
