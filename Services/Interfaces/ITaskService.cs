namespace TaskManagerAPI.Services.Interface
{
    public interface ITaskService
    {
        Task<List<Task>> GetTasksByProjectAsync(int projectId);
        Task<Task> CreateTaskAsync(Task task);
        Task<Task> UpdateTaskAsync(int id, Task updatedTask);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
    }
}
