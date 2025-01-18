// Controllers/TaskController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/Task
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] Task task)
    {
        var project = await _context.Projects.FindAsync(task.ProjectId);
        if (project == null)
        {
            return NotFound();
        }

        if (project.Tasks.Count >= 20)
        {
            return BadRequest("Limite de tarefas por projeto atingido.");
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    // GET: api/Task
    [HttpGet]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        return Ok(tasks);
    }

    // PUT: api/Task/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] Task task)
    {
        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask == null)
        {
            return NotFound();
        }

        existingTask.Status = task.Status;
        existingTask.Description = task.Description;

        // Add task update to history
        var history = new TaskHistory
        {
            FieldModified = "Status/Description",
            OldValue = existingTask.Status + "/" + existingTask.Description,
            NewValue = task.Status + "/" + task.Description,
            ModifiedDate = DateTime.UtcNow,
            ModifiedBy = "System",
            TaskId = id
        };
        _context.TaskHistories.Add(history);

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
