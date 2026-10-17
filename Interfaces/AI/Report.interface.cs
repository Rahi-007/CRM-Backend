public interface IAiReportService
{
    Task<AiReportResponse> GenerateSummaryAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken
    );

    Task<AiReportResponse> AskAsync(
        AiChatRequest request,
        CancellationToken cancellationToken
    );
}