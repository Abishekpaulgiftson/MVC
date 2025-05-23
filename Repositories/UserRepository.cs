﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc_1.Data;
using mvc_1.Repositories;

namespace mvc_1.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly AuthDbContext authDbContext;
         
        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>>GetAll()
        {
            var users = await authDbContext.Users.ToListAsync();
            var superAdminUser = await authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");
            if(superAdminUser!=null)
            {
                users.Remove(superAdminUser);
            }
            return users;
        }
    }
}
