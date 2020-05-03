using LoginApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginApp.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        public async Task<int> RegisterAsync(User u)
        {
            return await Add<User>(new
            {
                u.phone,
                u.password,
                u.token,
                u.activationCode
            });
        }

        public async Task<User> VerifyPhoneAsync(string code)
        {
            User user = await QueryFirstOrDefaultAsync<User>("VerifyPhone", new { code });
            return user;
        }

        public async Task<User> LoginAsync(string phone, string email)
        {
            User user = await QueryFirstOrDefaultAsync<User>("Login", new { phone, email });
            return user;
        }
    }
}
