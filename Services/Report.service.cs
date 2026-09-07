using Microsoft.EntityFrameworkCore;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DesignerReportDto>> GetDesignerReport(
        DesignerReportFilterDto filter)
    {
        var reports = await _context.Projects
            .AsNoTracking()
            .Where(p =>
                p.AssignedToId == filter.DesignerId &&
                p.SubmitDate >= filter.FromDate &&
                p.SubmitDate <= filter.ToDate
            )
            .Select(p => new DesignerReportDto
            {
                ProjectId = p.Id,
                ProjectName = p.Name,

                ClientId = p.ClientId,
                ClientName = p.Client.FirstName +
                             (p.Client.LastName != null
                                 ? " " + p.Client.LastName
                                 : ""),

                SubmitDate = p.SubmitDate,
                SubmitCode = p.SubmitCode,

                WorkType = p.WorkType,
                SubType = p.SubType,
                Quantity = p.Quantity,
                Status = p.Status,
                Link = p.Link
            })
            .OrderByDescending(p => p.SubmitDate)
            .ToListAsync();

        return reports;
    }

    public async Task<List<ClientReportDto>> GetClientReport(
        ClientReportFilterDto filter)
    {
        var reports = await _context.Projects
            .AsNoTracking()
            .Where(p =>
                p.ClientId == filter.ClientId &&
                p.SubmitDate >= filter.FromDate &&
                p.SubmitDate <= filter.ToDate
            )
            .Select(p => new ClientReportDto
            {
                ProjectId = p.Id,
                ProjectName = p.Name,

                AssignedToId = p.AssignedToId,
                AssignedToName = p.AssignedTo.FirstName +
                               (p.AssignedTo.LastName != null
                                   ? " " + p.AssignedTo.LastName
                                   : ""),

                WorkType = p.WorkType,
                SubType = p.SubType,
                Quantity = p.Quantity,
                SubmitDate = p.SubmitDate,
                SubmitCode = p.SubmitCode,
                Status = p.Status,
                Link = p.Link
            })
            .OrderByDescending(p => p.SubmitDate)
            .ToListAsync();

        return reports;
    }
    public async Task<List<TeamReportDto>> GetTeamReport(
        TeamReportFilterDto filter)
    {
        var reports = await _context.Projects
            .AsNoTracking()
            .Where(p =>
                p.AssignedTo.TeamId == filter.TeamId &&
                p.SubmitDate >= filter.FromDate &&
                p.SubmitDate <= filter.ToDate
            )
            .Select(p => new TeamReportDto
            {
                ProjectId = p.Id,
                ProjectName = p.Name,

                ClientId = p.ClientId,
                ClientName = p.Client.FirstName +
                             (p.Client.LastName != null
                                 ? " " + p.Client.LastName
                                 : ""),

                AssignedToId = p.AssignedToId,
                AssignedToName = p.AssignedTo.FirstName +
                               (p.AssignedTo.LastName != null
                                   ? " " + p.AssignedTo.LastName
                                   : ""),

                WorkType = p.WorkType,
                SubType = p.SubType,
                Quantity = p.Quantity,
                SubmitDate = p.SubmitDate,
                SubmitCode = p.SubmitCode,
                Status = p.Status,
                Link = p.Link
            })
            .OrderByDescending(p => p.SubmitDate)
            .ToListAsync();

        return reports;
    }

    public async Task<List<PerformerReportDto>> GetPerformerReport(
    PerformerReportFilterDto filter)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Where(p =>
                p.AssignedTo.TeamId == filter.TeamId &&
                p.SubmitDate >= filter.FromDate &&
                p.SubmitDate <= filter.ToDate &&
                (!filter.ClientId.HasValue ||
                 p.ClientId == filter.ClientId.Value)
            );

        List<PerformerReportDto> report = await query
            .GroupBy(p => new
            {
                DesignerId = p.AssignedToId,
                DesignerName = p.AssignedTo.FirstName +
                               " " +
                               (p.AssignedTo.LastName ?? ""),
                DesignerPhone = p.AssignedTo.Phone,
            })
            .Select(g => new PerformerReportDto
            {
                DesignerId = g.Key.DesignerId,
                DesignerName = g.Key.DesignerName.Trim(),
                DesignerPhone = g.Key.DesignerPhone.Trim(),
                TotalQuantity = g.Sum(p => p.Quantity),
                ApprovedQuantity = g
                    .Where(p =>
                        p.Status == ProjectStatus.BM_Approved ||
                        p.Status == ProjectStatus.HoM_Approved
                    )
                    .Sum(p => p.Quantity),
                SubmittedQuantity = g
                    .Where(p =>
                        p.Status == ProjectStatus.Brief_Submitted
                    )
                    .Sum(p => p.Quantity),
                InReviewQuantity = g
                    .Where(p =>
                        p.Status == ProjectStatus.In_Review
                    )
                    .Sum(p => p.Quantity),
                TotalProjects = g.Count(),
                ApprovalPercentage =
                    g.Sum(p => p.Quantity) == 0
                        ? 0
                        : Math.Round(
                            (decimal)g
                                .Where(p =>
                                    p.Status == ProjectStatus.BM_Approved ||
                                    p.Status == ProjectStatus.HoM_Approved
                                )
                                .Sum(p => p.Quantity)
                            / g.Sum(p => p.Quantity) * 100,
                            2
                        ),
                AverageQuantity =
                    g.Count() == 0
                        ? 0
                        : Math.Round(
                            (decimal)g.Sum(p => p.Quantity) / g.Count(),
                            2
                        ),

                WorkingHours = 0
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ToListAsync();

        return report;
    }
}