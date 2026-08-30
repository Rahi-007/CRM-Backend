public interface IReportService
{
    Task<List<DesignerReportDto>> GetDesignerReport(DesignerReportFilterDto filter);
    Task<List<ClientReportDto>> GetClientReport(ClientReportFilterDto filter);
    Task<List<TeamReportDto>> GetTeamReport(TeamReportFilterDto filter);
}