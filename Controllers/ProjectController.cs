// Controllers/ProjectController.cs
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Services.Interface;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: api/Project
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _projectService.GetProjectsAsync();
        return Ok(projects);
    }

    // POST: api/Project
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] Project project)
    {
        var newProject = await _projectService.CreateProjectAsync(project);
        return CreatedAtAction(nameof(GetProjects), new { id = newProject.Id }, newProject);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        } 
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
