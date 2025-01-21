// Controllers/TaskController.cs
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Services.Interface;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // POST: api/Task
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] Task task)
    {
        var createdTask = await _taskService.CreateTaskAsync(task);
        return CreatedAtAction(nameof(GetTasks), new { id = createdTask.Id }, createdTask);
    }

    // GET: api/Task
    [HttpGet]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var tasks = await _taskService.GetTasksByProjectAsync(projectId);
        return Ok(tasks);
    }

    // PUT: api/Task/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] Task task)
    {
        var existingTask = await _taskService.UpdateTaskAsync(id, task);
        return Ok(existingTask);
    }

    // DELETE: api/Task/5
    [HttpDelete]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
