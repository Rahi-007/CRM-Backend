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
}