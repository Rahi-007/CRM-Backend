using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ai-reports")]
[Authorize]
public sealed class AiReportsController : ControllerBase
{
    private readonly IAiReportService _aiReportService;

    public AiReportsController(IAiReportService aiReportService)
    {
        _aiReportService = aiReportService;
    }

    [HttpPost("summary")]
    public async Task<ActionResult<AiReportResponse>> Summary(
        [FromBody] AiSummaryRequest request,
        CancellationToken cancellationToken)
    {
        if (request.From > request.To)
            return BadRequest("From date cannot be after To date.");

        if (request.To.DayNumber - request.From.DayNumber > 366)
            return BadRequest("The selected period cannot exceed 367 days.");

        var result = await _aiReportService.GenerateSummaryAsync(
            request.From,
            request.To,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("chat")]
    public async Task<ActionResult<AiReportResponse>> Chat(
        [FromBody] AiChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest("Question is required.");

        if (request.Question.Length > 2000)
            return BadRequest("Question cannot exceed 2000 characters.");

        if (request.From > request.To)
            return BadRequest("From date cannot be after To date.");

        if (request.To.DayNumber - request.From.DayNumber > 366)
            return BadRequest("The selected period cannot exceed 367 days.");

        if (request.History is null)
            return BadRequest("History cannot be null.");

        if (request.History.Count > 10)
            return BadRequest("Maximum 10 history messages are allowed.");

        if (request.History.Any(m =>
                m is null ||
                (m.Role != "user" && m.Role != "assistant") ||
                m.Content is null ||
                m.Content.Length > 2000))
        {
            return BadRequest(
                "History contains an invalid message.");
        }

        var result = await _aiReportService.AskAsync(
            request,
            cancellationToken);

        return Ok(result);
    }
}