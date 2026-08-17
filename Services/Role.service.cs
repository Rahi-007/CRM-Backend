using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class RoleService : IRoleService
{
    public readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _appDbContext;
    public RoleService(AppDbContext appDbContext, IMapper mapper, ICurrentUserService currentUser)
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _appDbContext = appDbContext;
    }

    public async Task<List<PermissionResDto>> GetAllPermissions()
    {
        var Permissions = _appDbContext.Permissions.AsQueryable();
        return _mapper.Map<List<PermissionResDto>>(Permissions);
    }

    public async Task<List<RoleResDto>> GetAllRoles()
    {
        var query = _appDbContext.Roles
            .Include(t => t.Permissions)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .AsQueryable();

        var roles = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<RoleResDto>>(roles);
    }

    public async Task<List<SelectRoleRes>> SelectRoles()
    {
        return await _appDbContext.Roles
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .ProjectTo<SelectRoleRes>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<RoleResDto?> GetRoleById(int id)
    {
        Role? role = await _appDbContext.Roles
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);

        return role == null ? null : _mapper.Map<RoleResDto>(role);
    }

    public async Task<int> CreateRole(CreateRoleDto createData)
    {
        Role? existRole = await _appDbContext.Roles
            .FirstOrDefaultAsync(t => t.Name == createData.Name);

        if (existRole != null)
            throw new Exception("Designation Name is already Exist");

        var permissions = await _appDbContext.Permissions
            .Where(p => createData.PermissionIds.Contains(p.Id))
            .ToListAsync();

        if (permissions.Count != createData.PermissionIds.Distinct().Count())
            throw new Exception("Permissions are invalid");

        Role newRole = _mapper.Map<Role>(createData);

        newRole.CreatedById = _currentUser.UserId;
        newRole.Permissions = permissions;

        await _appDbContext.Roles.AddAsync(newRole);
        await _appDbContext.SaveChangesAsync();

        return newRole.Id;
    }

    public async Task<bool> UpdateRole(int id, CreateRoleDto updateData)
    {
        Role? role = await _appDbContext.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
            return false;

        if (!string.IsNullOrWhiteSpace(updateData.Name))
        {
            bool nameExists = await _appDbContext.Roles
                .AnyAsync(x =>
                    x.Name == updateData.Name &&
                    x.Id != id);

            if (nameExists)
                throw new Exception("Designation Name already exists");
        }

        var permissions = await _appDbContext.Permissions
            .Where(p => updateData.PermissionIds.Contains(p.Id))
            .ToListAsync();

        if (permissions.Count != updateData.PermissionIds.Distinct().Count())
            throw new Exception("Permissions are invalid");

        _mapper.Map(updateData, role);
        role.Permissions.Clear();

        foreach (var permission in permissions)
        {
            role.Permissions.Add(permission);
        }

        role.UpdatedById = _currentUser.UserId;
        role.UpdatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteRole(int id)
    {
        Role? role = await _appDbContext.Roles.FirstOrDefaultAsync(t => t.Id == id);
        if (role == null) return false;

        _appDbContext.Roles.Remove(role);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}