using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class ProjectService : IProjectService
{
    public readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _appDbContext;
    public ProjectService(AppDbContext appDbContext, IMapper mapper, ICurrentUserService currentUser)
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _appDbContext = appDbContext;
    }

    public async Task<List<ProjectResDto>> GetAllProjects()
    {
        var query = _appDbContext.Projects
            .Include(t => t.Client)
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .AsQueryable();

        var projects = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<ProjectResDto>>(projects);
    }

    public async Task<List<SelectProjectRes>> SelectProjects()
    {
        return await _appDbContext.Projects
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .ProjectTo<SelectProjectRes>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ProjectResDto?> GetProjectById(int id)
    {
        Project? project = await _appDbContext.Projects
            .Include(t => t.Client)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(u => u.Id == id);

        return project == null ? null : _mapper.Map<ProjectResDto>(project);
    }

    public async Task<int> CreateProject(CreateProjectDto createData)
    {
        Project? existTeam = await _appDbContext.Projects.FirstOrDefaultAsync(t => t.Name == createData.Name);
        if (existTeam != null) throw new Exception("Project name is already Exist");

        User? assignedUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == createData.AssignedToId);
        if (assignedUser?.TeamId == null) throw new Exception("Assigned user is not a member of any team");

        Project newProject = _mapper.Map<Project>(createData);
        newProject.CreatedById = _currentUser.UserId;

        await _appDbContext.Projects.AddAsync(newProject);
        await _appDbContext.SaveChangesAsync();

        return newProject.Id;
    }

    public async Task<bool> UpdateProject(int id, CreateProjectDto updateData)
    {
        Project? project = await _appDbContext.Projects
            .FirstOrDefaultAsync(x => x.Id == id);

        if (project == null) return false;

        if (!string.IsNullOrWhiteSpace(updateData.Name) &&
            updateData.Name != project.Name)
        {
            bool nameExists = await _appDbContext.Projects
                .AnyAsync(x =>
                    x.Name == updateData.Name &&
                    x.Id != id);

            if (nameExists)
                throw new Exception("Name already exists.");
        }

        // Assigned user changed
        if (project.AssignedToId != updateData.AssignedToId)
        {
            User? assignedUser = await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == updateData.AssignedToId);

            if (assignedUser?.TeamId == null)
                throw new Exception(
                    "Assigned user is not a member of any team."
                );

            User? oldUser = await _appDbContext.Users
                .Include(u => u.AssignedProjects)
                .FirstOrDefaultAsync(u => u.Id == project.AssignedToId);

            if (oldUser != null)
                oldUser.AssignedProjects.Remove(project);
        }

        // Client changed
        if (project.ClientId != updateData.ClientId)
        {
            Client? oldClient = await _appDbContext.Clients
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == project.ClientId);

            if (oldClient != null)
                oldClient.Projects.Remove(project);
        }

        _mapper.Map(updateData, project);

        project.UpdatedById = _currentUser.UserId;
        project.UpdatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProject(int id)
    {
        Project? project = await _appDbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.AssignedTo)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            return false;

        project.Client.Projects.Remove(project);
        project.AssignedTo.AssignedProjects.Remove(project);

        _appDbContext.Projects.Remove(project);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}