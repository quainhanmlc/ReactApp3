using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactApp3.Server.Models;
using ReactApp3.Server.Services;
using System;
using System.Threading.Tasks;


[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly ProjectService _projectService;
    private readonly UserService _userService;

    public EmployeeController(ProjectService projectService, UserService userService)
    {
        _projectService = projectService;
        _userService = userService;
    }

    // Tạo Project - Chỉ Nhân viên trong Team Sales
    [HttpPost("create-project")]
    public async Task<IActionResult> CreateProject([FromBody] Project project)
    {
        try
        {
            await _projectService.CreateProjectAsync(project);
            return Ok(new { message = "Project created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
