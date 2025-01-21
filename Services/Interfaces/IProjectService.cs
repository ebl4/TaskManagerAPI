namespace TaskManagerAPI.Services.Interface
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(Project project);
        System.Threading.Tasks.Task DeleteProjectAsync(int id);
    }
}
