using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public sealed class AiReportDataService : IAiReportDataService
{
    private readonly AppDbContext _context;

    public AiReportDataService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JsonElement> GetReportSnapshotAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        if (from > to)
        {
            throw new ArgumentException(
                "From date cannot be after To date.");
        }

        // DateOnly filter-কে timestamp range-এ রূপান্তর করা হয়েছে।
        // To date-এর পরের দিনের শুরু exclusive রাখা হয়েছে।
        var projectsQuery = _context.Projects
        .AsNoTracking()
        .Where(p =>
            p.SubmitDate >= from &&
            p.SubmitDate <= to);

        var totalProjects = await projectsQuery.CountAsync(
            cancellationToken);

        var totalQuantity = await projectsQuery
            .SumAsync(p => (int?)p.Quantity, cancellationToken) ?? 0;

        var statusBreakdown = await projectsQuery
            .GroupBy(p => p.Status)
            .Select(g => new
            {
                Status = g.Key.ToString(),
                ProjectCount = g.Count(),
                TotalQuantity = g.Sum(p => p.Quantity)
            })
            .ToListAsync(cancellationToken);

        var designerBreakdown = await projectsQuery
            .GroupBy(p => new
            {
                p.AssignedToId,
                p.AssignedTo.FirstName,
                p.AssignedTo.LastName
            })
            .Select(g => new
            {
                DesignerId = g.Key.AssignedToId,
                DesignerName =
                    g.Key.FirstName + " " + (g.Key.LastName ?? ""),
                ProjectCount = g.Count(),
                TotalQuantity = g.Sum(p => p.Quantity),
                ApprovedQuantity = g
                    .Where(p =>
                        p.Status == ProjectStatus.BM_Approved ||
                        p.Status == ProjectStatus.HoM_Approved)
                    .Sum(p => p.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(20)
            .ToListAsync(cancellationToken);

        var clientBreakdown = await projectsQuery
            .GroupBy(p => new
            {
                p.ClientId,
                p.Client.FirstName,
                p.Client.LastName
            })
            .Select(g => new
            {
                ClientId = g.Key.ClientId,
                ClientName =
                    g.Key.FirstName + " " + (g.Key.LastName ?? ""),
                ProjectCount = g.Count(),
                TotalQuantity = g.Sum(p => p.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(20)
            .ToListAsync(cancellationToken);

        var snapshot = new
        {
            Period = new
            {
                From = from,
                To = to
            },
            Summary = new
            {
                TotalProjects = totalProjects,
                TotalQuantity = totalQuantity
            },
            StatusBreakdown = statusBreakdown,
            TopDesigners = designerBreakdown,
            TopClients = clientBreakdown
        };

        return JsonSerializer.SerializeToElement(snapshot);
    }
}