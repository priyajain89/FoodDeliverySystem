//using FoodDelivery.Domain.Data;
//using Microsoft.EntityFrameworkCore;
//using FoodDelivery.Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FoodDelivery.Infrastructure.Repository
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly AppDbContext _context;

//        public UserRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<User>> GetAllUsersAsync()
//        {
//            return await _context.Users.ToListAsync();
//        }

//        public async Task<User> GetUserByIdAsync(int id)
//        {
//            return await _context.Users.FindAsync(id);
//        }

//        public async Task<User> CreateUserAsync(User user)
//        {
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//            return user;
//        }

//        public async Task<User> UpdateUserAsync(User user)
//        {
//            _context.Users.Update(user);
//            await _context.SaveChangesAsync();
//            return user;
//        }

//        public async Task<bool> DeleteUserAsync(int id)
//        {
//            var user = await _context.Users.FindAsync(id);
//            if (user == null) return false;

//            _context.Users.Remove(user);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//    }

//}
