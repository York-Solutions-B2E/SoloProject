using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.CommunicationDbContext;
using WebApi.Controllers;
using WebApi.Entities;
using WebApi.Services;

namespace WebApi.IntegrationTests {
    public static class PaginationTestHelper
    {
        public static async Task<IRequestExecutor> CreateExecutorAsync()
        {
            var services = new ServiceCollection();

            // Unique DB name per test
            var dbName = $"TestDb_{Guid.NewGuid()}";
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase(dbName));

            services.AddLogging();
            services.AddOptions();
            services.AddAuthorization();

            services.AddScoped<ICommunicationService, CommunicationService>();
            services.AddScoped<ICommunicationRepository, CommunicationRepository>();

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddAuthorization()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
                .AddFiltering()
                .AddSorting();

            var serviceProvider = services.BuildServiceProvider();

            // Seed deterministic data
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Communications.RemoveRange(db.Communications);
                db.CommunicationTypes.RemoveRange(db.CommunicationTypes);
                db.SaveChanges();

                var typeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                db.CommunicationTypes.Add(new CommunicationType
                {
                    Id = typeId,
                    TypeCode = "EOB",
                    DisplayName = "Test"
                });

                var baseTime = new DateTime(2025, 08, 20, 10, 0, 0, DateTimeKind.Utc);

                db.Communications.AddRange(
                    new Communication { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Title = "Comm1", CurrentStatus = "Open", LastUpdatedUtc = baseTime.AddSeconds(1), CommunicationTypeId = typeId },
                    new Communication { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Title = "Comm2", CurrentStatus = "Closed", LastUpdatedUtc = baseTime.AddSeconds(2), CommunicationTypeId = typeId },
                    new Communication { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Title = "Comm3", CurrentStatus = "Open", LastUpdatedUtc = baseTime.AddSeconds(3), CommunicationTypeId = typeId },
                    new Communication { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Title = "Comm4", CurrentStatus = "Closed", LastUpdatedUtc = baseTime.AddSeconds(4), CommunicationTypeId = typeId },
                    new Communication { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Title = "Comm5", CurrentStatus = "Open", LastUpdatedUtc = baseTime.AddSeconds(5), CommunicationTypeId = typeId }
                );

                db.SaveChanges();
            }

            return await serviceProvider
                .GetRequiredService<IRequestExecutorResolver>()
                .GetRequestExecutorAsync();
        }
    }
}
