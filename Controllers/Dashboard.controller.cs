using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // Get: api/v1/dashboard => dashboard report
    [HttpGet()]
    public async Task<IActionResult> GetDashboardReport()
    {
        var result = await _dashboardService.GetDashboardReport();
        return Ok(result);
    }
}