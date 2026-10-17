using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _appDbContext;
    public DashboardService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<DashboardDtoRes> GetDashboardReport()
    {
        var now = DateTime.UtcNow;

        var currentMonthStart = new DateTime(
            now.Year,
            now.Month,
            1,
            0,
            0,
            0,
            DateTimeKind.Utc
        );

        var nextMonthStart = currentMonthStart.AddMonths(1);
        var previousMonthStart = currentMonthStart.AddMonths(-1);


        // Total Users
        var totalUsers = await _appDbContext.Users
            .CountAsync();

        var usersThisMonth = await _appDbContext.Users
            .CountAsync(u =>
                u.CreatedAt >= currentMonthStart &&
                u.CreatedAt < nextMonthStart
            );

        var usersPreviousMonth = await _appDbContext.Users
            .CountAsync(u =>
                u.CreatedAt >= previousMonthStart &&
                u.CreatedAt < currentMonthStart
            );


        // User Growth Percentage
        double userGrowthPercentage = 0;

        if (usersPreviousMonth > 0)
        {
            userGrowthPercentage =
                ((double)(usersThisMonth - usersPreviousMonth)
                / usersPreviousMonth) * 100;
        }
        else if (usersThisMonth > 0)
        {
            userGrowthPercentage = 100;
        }

        // Total Projects
        var totalProjects = await _appDbContext.Projects
            .CountAsync();

        var projectsThisMonth = await _appDbContext.Projects
            .CountAsync(p =>
                p.SubmitDate >= DateOnly.FromDateTime(currentMonthStart) &&
                p.SubmitDate < DateOnly.FromDateTime(nextMonthStart)
            );


        // Completed Projects (HoM_Approved = Completed)
        var totalCompletedProjects = await _appDbContext.Projects
            .CountAsync(p =>
                p.Status == ProjectStatus.HoM_Approved
            );


        var completedProjectsThisMonth = await _appDbContext.Projects
            .CountAsync(p =>
                p.Status == ProjectStatus.HoM_Approved &&
                p.SubmitDate >= DateOnly.FromDateTime(currentMonthStart) &&
                p.SubmitDate < DateOnly.FromDateTime(nextMonthStart)
            );

        // Completion Rate This Month
        double completionRateThisMonth = 0;

        if (projectsThisMonth > 0)
        {
            completionRateThisMonth =
                ((double)completedProjectsThisMonth / projectsThisMonth) * 100;
        }

        // Total Clients
        var totalClients = await _appDbContext.Clients
            .CountAsync();

        var clientsThisMonth = await _appDbContext.Clients
            .CountAsync(c =>
                c.CreatedAt >= currentMonthStart &&
                c.CreatedAt < nextMonthStart
            );


        // Project Overview Graph
        var projectOverview = await _appDbContext.Projects
            .AsNoTracking()
            .GroupBy(p => p.Status)
            .Select(g => new ProjectStatusOverviewDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        // Recent 5 Projects List
        var recentProjects = await _appDbContext.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
            .Select(p => new RecentProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ClientName = p.Client.FirstName + "" + (p.Client.LastName ?? ""),
                DesignerName = p.AssignedTo.FirstName + " " +
                               (p.AssignedTo.LastName ?? ""),
                Status = p.Status,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        // Top 5 Performers - Last 12 Months Trend
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var currentMonth = new DateOnly(today.Year, today.Month, 1);

        var startMonth = currentMonth.AddMonths(-11);
        var endMonth = currentMonth.AddMonths(1);
        var months = Enumerable.Range(0, 12)
            .Select(i => startMonth.AddMonths(i))
            .ToList();

        // top 5 designers by total quantity 
        var topDesigners = await _appDbContext.Projects
            .AsNoTracking()
            .Where(p =>
                p.SubmitDate >= startMonth &&
                p.SubmitDate < endMonth)
            .GroupBy(p => new
            {
                p.AssignedToId,
                p.AssignedTo.FirstName,
                p.AssignedTo.LastName
            })
            .Select(g => new
            {
                DesignerId = g.Key.AssignedToId,
                DesignerName = (g.Key.FirstName + " " +
                                (g.Key.LastName ?? "")).Trim(),
                TotalQuantity = g.Sum(p => p.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(5)
            .ToListAsync();

        var topDesignerIds = topDesigners
            .Select(x => x.DesignerId)
            .ToList();

        // get monthly quantity for the designers
        var monthlyData = await _appDbContext.Projects
            .AsNoTracking()
            .Where(p =>
                topDesignerIds.Contains(p.AssignedToId) &&
                p.SubmitDate >= startMonth &&
                p.SubmitDate < endMonth)
            .GroupBy(p => new
            {
                p.AssignedToId,
                Month = p.SubmitDate.Year * 100 + p.SubmitDate.Month
            })
            .Select(g => new
            {
                DesignerId = g.Key.AssignedToId,
                Month = g.Key.Month.ToString(),
                TotalQuantity = g.Sum(p => p.Quantity)
            })
            .ToListAsync();

        var quantityLookup = monthlyData.ToDictionary(
            x => (x.DesignerId, x.Month),
            x => x.TotalQuantity
        );

        var topPerformerTrends = topDesigners
            .SelectMany(designer => months.Select(month =>
            {
                var monthKey = $"{month.Year}{month.Month:D2}";

                return new TopPerformerTrendDto
                {
                    DesignerId = designer.DesignerId,
                    DesignerName = designer.DesignerName,
                    Month = monthKey,
                    TotalQuantity = quantityLookup.TryGetValue(
                        (designer.DesignerId, monthKey),
                        out var quantity)
                            ? quantity
                            : 0
                };
            }))
            .OrderBy(x => x.Month)
            .ThenBy(x => x.DesignerName)
            .ToList();

        return new DashboardDtoRes
        {
            TotalUsers = totalUsers,
            UserGrowthPercentage = Math.Max(
                0,
                Math.Round(userGrowthPercentage, 2)
            ),

            TotalProjects = totalProjects,
            ProjectsThisMonth = projectsThisMonth,

            TotalCompletedProjects = totalCompletedProjects,
            CompletionRateThisMonth = Math.Round(
                completionRateThisMonth,
                2
            ),

            TotalClients = totalClients,
            ClientsThisMonth = clientsThisMonth,

            ProjectOverview = projectOverview,
            RecentProjects = recentProjects,
            TopPerformerTrends = topPerformerTrends
        };
    }
}