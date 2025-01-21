using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Services.Interface;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == task.ProjectId);

            if (project == null)
            {
                throw new KeyNotFoundException("Projeto não encontrado.");
            }

            if (project.Tasks.Count >= 20)
            {
                throw new InvalidOperationException("O limite de 20 tarefas por projeto foi atingido.");
            }

            task.Status = "Pendente";
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<List<Task>> GetTasksByProjectAsync(int projectId)
        {
            var project = await _context.Projects
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException("Projeto não encontrado.");

            return project.Tasks.ToList();
        }

        public async Task<Task> UpdateTaskAsync(int id, Task updatedTask)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada.");
            }

            if (existingTask.Priority != updatedTask.Priority)
                throw new InvalidOperationException("A prioridade da tarefa não pode ser alterada.");

            // Add task update to history
            var history = new TaskHistory
            {
                TaskId = id,
                ModifiedDate = DateTime.UtcNow,
                FieldModified = "Status/Description",
                OldValue = existingTask.Status + "/" + existingTask.Description,
                NewValue = updatedTask.Status + "/" + updatedTask.Description,
                ModifiedBy = "System",
            };

            existingTask.Title = updatedTask.Title;
            existingTask.Status = updatedTask.Status;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;

            _context.TaskHistories.Add(history);
            await _context.SaveChangesAsync();

            return existingTask;
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            _context.Tasks.Remove(existingTask);
            await _context.SaveChangesAsync();
        }
    }
}
