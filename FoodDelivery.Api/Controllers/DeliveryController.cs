using FoodDelivery.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {

        
            private readonly IDeliveryagent _repo;

            public DeliveryController(IDeliveryagent repo)
            {
                _repo = repo;
            }

            [HttpPost("submit")]
            public async Task<IActionResult> SubmitDetails([FromBody] DeliveryAgentDTO dto)
            {
                var agent = new DeliveryAgent
                {
                    UserId = dto.UserId,
                    DocumentUrl = dto.DocumentUrl,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Address = dto.Address
                };

                try
                {
                    var result = await _repo.SubmitAgentDetailsAsync(agent);

                    var response = new DeliveryAgentResponseDto
                    {
                        AgentId = result.AgentId,
                        UserId = result.UserId ?? 0,
                        DocumentUrl = result.DocumentUrl,
                        Latitude = result.Latitude,
                        Longitude = result.Longitude,
                        Address = result.Address
                    };

                    return Ok(response);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }





