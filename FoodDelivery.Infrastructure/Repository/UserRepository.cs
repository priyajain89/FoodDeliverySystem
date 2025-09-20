using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.Repository;
using FoodDelivery.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly OtpService _otpService;

        public UserRepository(AppDbContext context, OtpService otpService)
        {
            _context = context;
            _otpService = otpService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailOrPhoneAsync(string email, string phone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email || u.Phone == phone);
        }


        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //OTP logic 
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> LoginAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(r => r.Email == email);
            if (user == null)
            {
                return null; 
            }
            return user;
        }
      
        public async Task<string?> GenerateOtpAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return null;

            return await _otpService.GenerateOtpAsync(email);
        }

        public async Task<User?> VerifyOtpAsync(string email, string otp)
        {
            if (_otpService.VerifyOtp(email, otp))
            {
                return await GetUserByEmailAsync(email);
            }
            return null;
        }


    }
}

