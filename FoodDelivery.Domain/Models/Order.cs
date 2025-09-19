using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? RestaurantId { get; set; }

    public int? AgentId { get; set; }

    public int? AddressId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Orders")]
    public virtual Address? Address { get; set; }

    [ForeignKey("AgentId")]
    [InverseProperty("Orders")]
    public virtual DeliveryAgent? Agent { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("RestaurantId")]
    [InverseProperty("Orders")]
    public virtual Restaurant? Restaurant { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}
