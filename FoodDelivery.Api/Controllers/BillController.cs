using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet("bill/order/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetBillFromOrder(int orderId) 
        {
            var bill = await _billService.GenerateBillFromOrderAsync(orderId);
            return Ok(bill);
        }
    }
}
