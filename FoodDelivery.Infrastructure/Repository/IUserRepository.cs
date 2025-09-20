
﻿using FoodDelivery.Domain.Models;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByEmailOrPhoneAsync(string email, string phone);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);


        Task<User> LoginAsync(string email);
        Task<string?> GenerateOtpAsync(string email);
        Task<User?> VerifyOtpAsync(string email, string otp);


    }
}

