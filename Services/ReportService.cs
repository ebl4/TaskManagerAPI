using Microsoft.EntityFrameworkCore;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;
    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Performance>> GenerateReport(bool isManager)
    {
        if (!isManager)
            throw new InvalidOperationException("Acesso restrito a gerentes");

        var limitDate = DateTime.Now.AddDays(-30);
        var performance = await _context.Tasks
            .Where(t => t.Status == "Concluída" && t.DueDate >= limitDate)
            .GroupBy(t => t.Project.Id)
            .Select(g => new Performance
            {
                UserId = g.Key,
                TasksDone = g.Count()
            })
            .ToListAsync();

        return performance;
    }
}