using Microsoft.AspNetCore.Identity;

namespace Bloggi.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
