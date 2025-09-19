using FoodDelivery.Domain.Models;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IDeliveryagent
    {
        Task<DeliveryAgent> SubmitAgentDetailsAsync(DeliveryAgent agent);
    }
}
