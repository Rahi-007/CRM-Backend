using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/reports")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    // Get: api/v1/reports/designer ? => designer wise report
    [HttpGet("designer")]
    public async Task<IActionResult> GetDesignerReport(
        [FromQuery] DesignerReportFilterDto filter)
    {
        var result = await _reportService.GetDesignerReport(filter);
        return Ok(result);
    }

    // Get: api/v1/reports/client ? => client wise report
    [HttpGet("client")]
    public async Task<IActionResult> GetClientReport(
        [FromQuery] ClientReportFilterDto filter)
    {
        var result = await _reportService.GetClientReport(filter);
        return Ok(result);
    }

    // Get: api/v1/reports/team ? => team wise report
    [HttpGet("team")]
    public async Task<IActionResult> GetTeamReport(
        [FromQuery] TeamReportFilterDto filter)
    {
        var result = await _reportService.GetTeamReport(filter);
        return Ok(result);
    }

    // Get: api/v1/reports/performer ? => top performer report
    [HttpGet("performer")]
    public async Task<IActionResult> GetPerformerReport(
        [FromQuery] PerformerReportFilterDto filter)
    {
        var result = await _reportService.GetPerformerReport(filter);
        return Ok(result);
    }
}