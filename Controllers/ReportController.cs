// Controllers/ReportController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("desempenho")]
    public async Task<IActionResult> GenerateReport()
    {
        var isManager = true;
        var performance = await _reportService.GenerateReport(isManager);

        return Ok(performance);

    }

}
