using MongoDB.Driver;
using ReactApp3.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReactApp3.Server.Services
{


    public class TeamService
    {
        private readonly IMongoCollection<Team> _teams;

        public TeamService(IMongoDatabase database)
        {
            _teams = database.GetCollection<Team>("Team");
        }

        // 1. Lấy tất cả các phòng ban
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _teams.Find(_ => true).ToListAsync();
        }

        // 2. Lấy phòng ban theo ID
        public async Task<Team> GetTeamByIdAsync(string id)
        {
            return await _teams.Find(team => team.Id == id).FirstOrDefaultAsync();
        }

        // 3. Tạo phòng ban mới
        public async Task CreateTeamAsync(Team team)
        {
            team.CreateTime = DateTime.Now;
            team.UpdateTime = DateTime.Now;
            await _teams.InsertOneAsync(team);
        }

        // 4. Cập nhật thông tin phòng ban
        public async Task UpdateTeamAsync(Team team)
        {
            team.UpdateTime = DateTime.Now;
            var filter = Builders<Team>.Filter.Eq(t => t.Id, team.Id);
            await _teams.ReplaceOneAsync(filter, team);
        }

        // 5. Xóa phòng ban
        public async Task DeleteTeamAsync(string id)
        {
            var filter = Builders<Team>.Filter.Eq(t => t.Id, id);
            await _teams.DeleteOneAsync(filter);
        }

        // 6. Thay đổi trưởng phòng
        public async Task ChangeTeamLeadAsync(string teamId, string newManagerId)
        {
            var team = await GetTeamByIdAsync(teamId);
            if (team != null)
            {
                team.ManagerId = newManagerId;
                await UpdateTeamAsync(team);
            }
        }
    }

}
