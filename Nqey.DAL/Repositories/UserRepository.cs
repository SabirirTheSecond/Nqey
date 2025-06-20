﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;

namespace Nqey.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _dataContext.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return user;
            
        }

       public async Task<User> DeleteUserAsync(int id)
        {
            var toDeleteUser = await _dataContext.Users
                .FirstOrDefaultAsync(u=>u.UserId == id);
            if (toDeleteUser == null)
                return null;
            _dataContext.Users.Remove(toDeleteUser);
            await _dataContext.SaveChangesAsync();

            return toDeleteUser;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                 throw new NullReferenceException();
            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _dataContext.Users
                
                .ToListAsync();
            if (users == null)
                return null;

            return users;
            
        }
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u=> u.UserName == userName);
            if (user == null)
                return null;
            return user;
        }

       public async Task<User> UpdateUserAsync(User user)
        {
             _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
            return user;
            
        }

        public async Task<int?> GetUserIdByUserNameAsync(string userName)
        {
            var userId = await _dataContext.Users
                .Where(c => c.UserName == userName)
                .Select(c => (int?)c.UserId)
                .FirstOrDefaultAsync();
            if (userId == null)
                return null;

            return userId;
        }
    }
}
