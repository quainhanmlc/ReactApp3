using MongoDB.Driver;
using ReactApp3.Server.Models;
using System;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Security;

namespace ReactApp3.Server.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Permission> _permissions;
        private readonly IMongoCollection<Role> _roles;

        public UserService(IMongoDatabase database, string collectionName)
        {
            _users = database.GetCollection<User>(collectionName);
            _permissions = database.GetCollection<Permission>("Permission");
            _roles = database.GetCollection<Role>("Role");
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            var existingUser = await _users.Find(u => u.Email == email || u.Username == username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password.");
            }

            return user;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            // Lấy tất cả các permissions của user
            var permissions = await _permissions.Find(p => p.UserId == userId).ToListAsync();

            Console.WriteLine($"Permissions: {permissions.Count}");

            if (permissions.Count > 0)
            {
                foreach (var permission in permissions)
                {
                    Console.WriteLine($"Permission RoleId: {permission.RoleId}");
                }
            }

            // Lấy danh sách RoleId từ các permissions
            var roleIds = permissions.Select(p => p.RoleId).ToList();

            // Lấy danh sách các Role tương ứng với RoleId
            var roles = await _roles.Find(r => roleIds.Contains(r.Id)).ToListAsync();

            // Trả về danh sách RoleCode
            return roles.Select(r => r.RoleCode).ToList();
        }


        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            var resetToken = Guid.NewGuid().ToString();
            user.PasswordResetToken = resetToken;
            user.PasswordResetExpiry = DateTime.Now.AddMinutes(30);
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return resetToken;
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _users.Find(u => u.Email == email && u.PasswordResetToken == token && u.PasswordResetExpiry > DateTime.Now).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("Invalid token or token expired.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.PasswordResetExpiry = null;

            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task UpdatePasswordAsync(User user, string newPassword)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _users.ReplaceOneAsync(filter, user);
        }
    }
}
