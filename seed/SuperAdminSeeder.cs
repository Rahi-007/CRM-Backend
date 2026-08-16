using Microsoft.EntityFrameworkCore;

public static class SuperAdminSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Permission seed
        var permissions = await PermissionSeeder.SeedAsync(context);

        // Super Admin ID generate 
        User? admin = await context.Users
            .FirstOrDefaultAsync(u => u.Phone == "01729249260");

        Guid adminId;

        if (admin == null)
        {
            adminId = Guid.NewGuid();
        }
        else
        {
            adminId = admin.Id;
        }

        // Super Admin Role
        Role? superAdminRole = await context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == "Super Admin");

        // Role create
        if (superAdminRole == null)
        {
            superAdminRole = new Role
            {
                Id = 1,
                Name = "Super Admin",
                Narration = "Full system access",
                Permissions = permissions,
                CreatedById = adminId
            };

            context.Roles.Add(superAdminRole);

            await context.SaveChangesAsync();
        }
        else
        {
            // Existing role include all permissions
            superAdminRole.Permissions = permissions;
            await context.SaveChangesAsync();
        }

        // Super Admin User create
        if (admin == null)
        {
            admin = new User
            {
                Id = adminId,
                FirstName = "Super",
                LastName = "Admin",
                Phone = "01729249260",
                Address = string.Empty,
                Gender = Gender.Male,
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                RoleId = superAdminRole.Id,
                CreatedById = adminId
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
        else if (admin.RoleId != superAdminRole.Id)
        {
            admin.RoleId = superAdminRole.Id;
            await context.SaveChangesAsync();
        }
    }
}