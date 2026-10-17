public class DashboardDtoRes
{
    public int TotalUsers { get; set; }
    public double UserGrowthPercentage { get; set; }

    public int TotalProjects { get; set; }
    public int ProjectsThisMonth { get; set; }

    public int TotalCompletedProjects { get; set; }
    public double CompletionRateThisMonth { get; set; }

    public int TotalClients { get; set; }
    public int ClientsThisMonth { get; set; }

    public List<ProjectStatusOverviewDto> ProjectOverview { get; set; } = [];
    public List<RecentProjectDto> RecentProjects { get; set; } = [];
    public List<TopPerformerTrendDto> TopPerformerTrends { get; set; } = [];
}

public class ProjectStatusOverviewDto
{
    public ProjectStatus Status { get; set; }
    public int Count { get; set; }
}

public class RecentProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string DesignerName { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TopPerformerTrendDto
{
    public Guid DesignerId { get; set; }
    public string DesignerName { get; set; } = string.Empty;
    public string Month { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
}