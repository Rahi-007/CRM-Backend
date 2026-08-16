using Microsoft.EntityFrameworkCore;

public static class PermissionSeeder
{
    public static async Task<List<Permission>> SeedAsync(AppDbContext context)
    {
        var permissions = new List<Permission>
        {
            // Users
            new()
            {
                Name = "users.view",
                Module = "Users",
                Action = "View"
            },
            new()
            {
                Name = "users.create",
                Module = "Users",
                Action = "Create"
            },
            new()
            {
                Name = "users.edit",
                Module = "Users",
                Action = "Edit"
            },
            new()
            {
                Name = "users.delete",
                Module = "Users",
                Action = "Delete"
            },

            // Teams
            new()
            {
                Name = "teams.view",
                Module = "Teams",
                Action = "View"
            },
            new()
            {
                Name = "teams.create",
                Module = "Teams",
                Action = "Create"
            },
            new()
            {
                Name = "teams.edit",
                Module = "Teams",
                Action = "Edit"
            },
            new()
            {
                Name = "teams.delete",
                Module = "Teams",
                Action = "Delete"
            },

            // Projects
            new()
            {
                Name = "projects.view",
                Module = "Projects",
                Action = "View"
            },
            new()
            {
                Name = "projects.create",
                Module = "Projects",
                Action = "Create"
            },
            new()
            {
                Name = "projects.edit",
                Module = "Projects",
                Action = "Edit"
            },
            new()
            {
                Name = "projects.delete",
                Module = "Projects",
                Action = "Delete"
            },

            // Clients
            new()
            {
                Name = "clients.view",
                Module = "Clients",
                Action = "View"
            },
            new()
            {
                Name = "clients.create",
                Module = "Clients",
                Action = "Create"
            },
            new()
            {
                Name = "clients.edit",
                Module = "Clients",
                Action = "Edit"
            },
            new()
            {
                Name = "clients.delete",
                Module = "Clients",
                Action = "Delete"
            },

            // Roles
            new()
            {
                Name = "roles.view",
                Module = "Roles",
                Action = "View"
            },
            new()
            {
                Name = "roles.create",
                Module = "Roles",
                Action = "Create"
            },
            new()
            {
                Name = "roles.edit",
                Module = "Roles",
                Action = "Edit"
            },
            new()
            {
                Name = "roles.delete",
                Module = "Roles",
                Action = "Delete"
            },

            // System Settings
            new()
            {
                Name = "system.settings",
                Module = "System",
                Action = "Settings"
            }
        };

        var existingNames = await context.Permissions
            .Select(p => p.Name)
            .ToListAsync();

        var newPermissions = permissions
            .Where(p => !existingNames.Contains(p.Name))
            .ToList();

        if (newPermissions.Count > 0)
        {
            context.Permissions.AddRange(newPermissions);
            await context.SaveChangesAsync();
        }

        return await context.Permissions.ToListAsync();
    }
}