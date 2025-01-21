using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Services.Interface;

namespace TaskManagerAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _context.Projects.Include(p => p.Tasks).ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects.Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                throw new KeyNotFoundException("Projeto não encontrado.");

            return project;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
        {
            var existingProject = await GetProjectByIdAsync(id);

            if (existingProject.Tasks.Any(t => t.Status != "Concluída"))
                throw new InvalidOperationException("Não é possível remover um projeto com tarefas pendentes.");

            _context.Projects.Remove(existingProject);
            await _context.SaveChangesAsync();
        }
    }
}
