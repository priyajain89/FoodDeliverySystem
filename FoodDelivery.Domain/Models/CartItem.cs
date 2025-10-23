using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

public partial class CartItem
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }


    [JsonIgnore]
    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart? Cart { get; set; }

    
    [ForeignKey("ItemId")]
    [InverseProperty("CartItems")]

   
    public virtual MenuItem? Item { get; set; }
}
