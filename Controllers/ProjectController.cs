// Controllers/ProjectController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Project
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _context.Projects.ToListAsync();
        return Ok(projects);
    }

    // POST: api/Project
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }

    // PUT action
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Project project)
    {
        if (id != project.Id) return BadRequest();

        var existingProj = await _context.Projects.SingleOrDefaultAsync(c => c.Id == id);

        if (existingProj is not null)
        {
            existingProj.Id = project.Id;
            existingProj.Name = project.Name;
            existingProj.Tasks = project.Tasks;
            await _context.SaveChangesAsync();
            return Ok(existingProj);
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var existingProject = await _context.Projects.SingleOrDefaultAsync(m => m.Id == id);

        if (existingProject is not null)
        {
            _context.Remove(existingProject);
            await _context.SaveChangesAsync();
            return base.Ok(existingProject);
        }
        
        return NotFound();
    }

}
