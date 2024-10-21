using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactApp3.Server.Models;
using ReactApp3.Server.Helpers;
using ReactApp3.Server.Models.DTO;
using ReactApp3.Server.Services;

namespace ReactApp3.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _teamService;
        private readonly UserService _userService;


        public TeamController(TeamService teamService, UserService userService)
        {
            _userService = userService;
            _teamService = teamService;
        }

        // 1. Lấy tất cả các phòng ban
        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        // 2. Lấy thông tin phòng ban theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(string id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        // 3. Tạo mới phòng ban
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] Team team)
        {
            await _teamService.CreateTeamAsync(team);
            return Ok(new { message = "Team created successfully" });
        }

        // 4. Cập nhật phòng ban
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(string id, [FromBody] Team team)
        {
            team.Id = id;
            await _teamService.UpdateTeamAsync(team);
            return Ok(new { message = "Team updated successfully" });
        }

        // 5. Xóa phòng ban
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            await _teamService.DeleteTeamAsync(id);
            return Ok(new { message = "Team deleted successfully" });
        }
        [HttpPut("{teamId}/change-lead/{newManagerId}")]
        public async Task<IActionResult> ChangeTeamLead(string teamId, string newManagerId)
        {
            var team = await _teamService.GetTeamByIdAsync(teamId);
            if (team == null) return NotFound(new { message = "Team not found" });

            team.ManagerId = newManagerId;
            await _teamService.UpdateTeamAsync(team);
            return Ok(new { message = "Team lead changed successfully" });
        }
        [HttpPut("transfer-user/{userId}/to-team/{teamId}")]
        public async Task<IActionResult> TransferUserToTeam(string userId, string teamId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound(new { message = "User not found" });

            user.TeamId = teamId;
            await _userService.UpdateUserAsync(user);
            return Ok(new { message = "User transferred to new team successfully" });
        }

    }

}
