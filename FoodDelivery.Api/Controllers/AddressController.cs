using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository repository)
        {
            _addressRepository = repository;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAll()
        {
            var customerId = GetCustomerIdFromToken();
            if (customerId == null) return Unauthorized("Customer ID not found in token.");

            var addresses = await _addressRepository.GetAllByUserIdAsync(customerId.Value);
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetById(int id)
        {
            var customerId = GetCustomerIdFromToken();
            if (customerId == null) return Unauthorized("Customer ID not found in token.");

            var address = await _addressRepository.GetByIdForUserAsync(id, customerId.Value);
            if (address == null) return NotFound();
            return Ok(address);
        }

        [HttpGet("my-addresses")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var customerId = GetCustomerIdFromToken();
            if (customerId == null) return Unauthorized("Customer ID not found in token.");

            var addresses = await _addressRepository.GetAllByUserIdAsync(customerId.Value);
            return Ok(addresses);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddAddress(AddressAddDto dto)
        {
            var result = await _addressRepository.AddAddressAsync(dto, User);
            if (result == null)
                return Forbid("Unauthorized or invalid token.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(int id, Address address)
        {
            if (id != address.AddressId) return BadRequest();

            await _addressRepository.UpdateAsync(address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete(int id)
        {
            await _addressRepository.DeleteAsync(id);
            return NoContent();
        }

        // ✅ Helper method to extract customer ID from JWT token
        private int? GetCustomerIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "id");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }
    }
}
