public interface ITeamService
{
    Task<List<TeamResDto>> GetAllTeams();
    Task<List<SelectTeamRes>> SelectTeams();
    Task<TeamResDto?> GetTeamById(int id);
    Task<int> CreateTeam(CreateTeamDto createData);
    Task<bool> UpdateTeam(int id, CreateTeamDto updateData);
    Task<bool> DeleteTeam(int id);
}