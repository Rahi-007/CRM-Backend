
// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace CRM.Tests;

// public class UnitTest1
// {
//     [Fact]
//     public async Task TopPerformers_ShouldReturn12MonthsForTop5Designers()
//     {
//         // Arrange: temporary database
//         await using var connection =
//             new SqliteConnection("Data Source=:memory:");

//         await connection.OpenAsync();

//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseSqlite(connection)
//             .Options;

//         await using var context = new AppDbContext(options);

//         await context.Database.EnsureCreatedAsync();

//         // TODO:
//         // Seed User and Project entities here.
//         // Create at least 6 designers with projects
//         // in the last 12 months.

//         // TODO: Create your actual report service.
//         // var service = new YourReportService(context, ...);

//         // Act
//         // var result = await service.GetTopPerformerTrendsAsync();

//         // Assert
//         // Assert.Equal(60, result.Count);
//         // Assert.Equal(5, result.Select(x => x.DesignerId).Distinct().Count());

//         // Each designer should have 12 months.
//         // Assert.All(
//         //     result.GroupBy(x => x.DesignerId),
//         //     group => Assert.Equal(12, group.Count())
//         // );
//     }
// }
