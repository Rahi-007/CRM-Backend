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


        // ============================================
        // User
        // ============================================

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


        // ============================================
        // User Growth Percentage
        // ============================================

        double userGrowthPercentage = 0;

        if (usersPreviousMonth > 0)
        {
            userGrowthPercentage =
                ((double)(usersThisMonth - usersPreviousMonth)
                / usersPreviousMonth) * 100;
        }
        else if (usersThisMonth > 0)
        {
            // Previous month had no users,
            // but current month has users.
            userGrowthPercentage = 100;
        }


        // ============================================
        // Project
        // ============================================

        var totalProjects = await _appDbContext.Projects
            .CountAsync();

        var projectsThisMonth = await _appDbContext.Projects
            .CountAsync(p =>
                p.SubmitDate >= DateOnly.FromDateTime(currentMonthStart) &&
                p.SubmitDate < DateOnly.FromDateTime(nextMonthStart)
            );


        // ============================================
        // Completed Projects
        // BM_Approved = Completed
        // ============================================

        var totalCompletedProjects = await _appDbContext.Projects
            .CountAsync(p =>
                p.Status == ProjectStatus.BM_Approved
            );


        var completedProjectsThisMonth = await _appDbContext.Projects
            .CountAsync(p =>
                p.Status == ProjectStatus.BM_Approved &&
                p.SubmitDate >= DateOnly.FromDateTime(currentMonthStart) &&
                p.SubmitDate < DateOnly.FromDateTime(nextMonthStart)
            );


        // ============================================
        // Completion Rate This Month
        // ============================================

        double completionRateThisMonth = 0;

        if (projectsThisMonth > 0)
        {
            completionRateThisMonth =
                ((double)completedProjectsThisMonth / projectsThisMonth) * 100;
        }


        // ============================================
        // Client
        // ============================================

        var totalClients = await _appDbContext.Clients
            .CountAsync();

        var clientsThisMonth = await _appDbContext.Clients
            .CountAsync(c =>
                c.CreatedAt >= currentMonthStart &&
                c.CreatedAt < nextMonthStart
            );


        // ============================================
        // Response
        // ============================================

        return new DashboardDtoRes
        {
            TotalUsers = totalUsers,
            UserGrowthPercentage = Math.Max(0, Math.Round(userGrowthPercentage, 2)),

            TotalProjects = totalProjects,
            ProjectsThisMonth = projectsThisMonth,

            TotalCompletedProjects = totalCompletedProjects,
            CompletionRateThisMonth = Math.Round(
                completionRateThisMonth,
                2
            ),

            TotalClients = totalClients,
            ClientsThisMonth = clientsThisMonth
        };
    }
}