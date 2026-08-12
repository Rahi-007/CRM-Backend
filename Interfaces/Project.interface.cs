public interface IProjectService
{
    Task<List<ProjectResDto>> GetAllProjects();
    Task<List<SelectProjectRes>> SelectProjects();
    Task<ProjectResDto?> GetProjectById(int id);
    Task<int> CreateProject(CreateProjectDto createData);
    Task<bool> UpdateProject(int id, CreateProjectDto updateData);
    Task<bool> DeleteProject(int id);
}