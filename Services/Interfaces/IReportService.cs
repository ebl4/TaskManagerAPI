public interface IReportService
{
    Task<List<Performance>> GenerateReport(bool isManager);
}