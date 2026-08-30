public class DesignerReportDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public WorkType WorkType { get; set; }
    public SubType? SubType { get; set; }
    public int Quantity { get; set; }
    public DateOnly SubmitDate { get; set; }
    public string SubmitCode { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public string? Link { get; set; }
}
public class DesignerReportFilterDto
{
    public Guid DesignerId { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}

public class ClientReportDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public Guid AssignedToId { get; set; }
    public string AssignedToName { get; set; } = string.Empty;
    public WorkType WorkType { get; set; }
    public SubType? SubType { get; set; }
    public int Quantity { get; set; }
    public DateOnly SubmitDate { get; set; }
    public string SubmitCode { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public string? Link { get; set; }
}
public class ClientReportFilterDto
{
    public Guid ClientId { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}

public class TeamReportDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public Guid AssignedToId { get; set; }
    public string AssignedToName { get; set; } = string.Empty;
    public WorkType WorkType { get; set; }
    public SubType? SubType { get; set; }
    public int Quantity { get; set; }
    public DateOnly SubmitDate { get; set; }
    public string SubmitCode { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public string? Link { get; set; }
}
public class TeamReportFilterDto
{
    public int TeamId { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}
