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
}