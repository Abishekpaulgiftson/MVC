using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace mvc_1.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
