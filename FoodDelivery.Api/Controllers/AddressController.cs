using FoodDelivery.Domain.Models;

using FoodDelivery.Infrastructure.DTO;

using FoodDelivery.Infrastructure.Repository;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

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

        private int? GetUserIdFromToken()

        {

            var claim = User.Claims.FirstOrDefault(c => c.Type == "id");

            if (claim != null && int.TryParse(claim.Value, out var userId))

            {

                return userId;

            }

            return null;

        }


        [HttpGet]

        [Authorize(Roles = "customer")]

        public async Task<IActionResult> GetAll()

        {

            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            var addresses = await _addressRepository.GetAllByUserIdAsync(userId);

            return Ok(addresses);

        }


        [HttpGet("{id}")]

        [Authorize(Roles = "customer")]

        public async Task<IActionResult> GetById(int id)

        {

            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            var address = await _addressRepository.GetByUserIdForCustomerAsync(id, userId);

            if (address == null) return NotFound();

            return Ok(address);

        }


        [HttpGet("my-addresses")]

        [Authorize(Roles = "customer")]

        public async Task<IActionResult> GetMyAddresses()

        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");

            if (userIdClaim == null)

                return Unauthorized("Customer ID not found in token.");

            int userId = int.Parse(userIdClaim.Value);

            var addresses = await _addressRepository.GetAllByUserIdAsync(userId);

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

        [Authorize(Roles = "customer")]

        public async Task<IActionResult> Update(int id, [FromBody] AddressAddDto dto)

        {

            var userId = GetUserIdFromToken();

            if (userId == null) return Unauthorized("User ID not found in token.");

            var existing = await _addressRepository.GetByUserIdForCustomerAsync(id, userId.Value);

            if (existing == null) return NotFound("Address not found for this user.");

            var updatedAddress = new Address

            {

                AddressId = id,

                UserId = userId,

                AddressLine1 = dto.AddressLine1,

                AddressLine2 = dto.AddressLine2,

                City = dto.City,

                State = dto.State,

                PinCode = dto.PinCode,

                Landmark = dto.Landmark,

                IsDefault = dto.IsDefault

            };

            await _addressRepository.UpdateAsync(id, dto, User);

            return NoContent();

        }


        [HttpDelete("{id}")]

        [Authorize(Roles = "customer")]

        public async Task<IActionResult> Delete(int id)

        {

            await _addressRepository.DeleteAsync(id);

            return NoContent();

        }

    }

}

