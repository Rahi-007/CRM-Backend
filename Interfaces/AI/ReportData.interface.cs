using System.Text.Json;

public interface IAiReportDataService
{
    Task<JsonElement> GetReportSnapshotAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken
    );
}