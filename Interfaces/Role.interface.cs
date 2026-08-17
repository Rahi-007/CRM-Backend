public interface IRoleService
{
    Task<List<PermissionResDto>> GetAllPermissions();
    Task<List<RoleResDto>> GetAllRoles();
    Task<List<SelectRoleRes>> SelectRoles();
    Task<RoleResDto?> GetRoleById(int id);
    Task<int> CreateRole(CreateRoleDto createData);
    Task<bool> UpdateRole(int id, CreateRoleDto updateData);
    Task<bool> DeleteRole(int id);
}