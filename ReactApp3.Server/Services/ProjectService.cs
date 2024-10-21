using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactApp3.Server.Models;

public class ProjectService
{
    private readonly IMongoCollection<Project> _projects;

    public ProjectService(IMongoDatabase database)
    {
        _projects = database.GetCollection<Project>("Project");
    }

    // Tạo mới một Project
    public async Task CreateProjectAsync(Project project)
    {
        await _projects.InsertOneAsync(project);
    }

    // Lấy danh sách tất cả các Project
    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _projects.Find(_ => true).ToListAsync();
    }
}
