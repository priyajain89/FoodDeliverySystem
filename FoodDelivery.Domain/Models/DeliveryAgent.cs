using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Domain.Models;

public partial class DeliveryAgent
{
    [Key]
    public int AgentId { get; set; }

    public int? UserId { get; set; }

    public bool? IsAvailable { get; set; }

    public string? DocumentUrl { get; set; }

    [StringLength(50)]
    public string? Latitude { get; set; }

    [StringLength(50)]
    public string? Longitude { get; set; }

    public string? Address { get; set; }

    [InverseProperty("Agent")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("UserId")]
    [InverseProperty("DeliveryAgents")]
    public virtual User? User { get; set; }
}
