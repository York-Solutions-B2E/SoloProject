using System.Text.Json;
using CookieCrumble;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.CommunicationDbContext;
using WebApi.Controllers;
using WebApi.Entities;
using WebApi.Services;
using Xunit;

namespace WebApi.IntegrationTests
{
    // Disable parallelization to avoid EF InMemory collisions
    
    public class CommunicationsPaginationTests
    {
        
        [Fact]
        public async Task Page1_ReturnsTwoItems_HasNextPageTrue()
        {
            var executor = await PaginationTestHelper.CreateExecutorAsync();
            var query = @"
                query {
                    communications(first: 2) {
                        edges { cursor node { id title } }
                        pageInfo { hasNextPage endCursor }
                    }
                }";

            var result = await executor.ExecuteAsync(query);

            // Stable snapshot
            result.ToJson().MatchSnapshot();

            using var doc = JsonDocument.Parse(result.ToJson());
            var edges = doc.RootElement.GetProperty("data")
                                       .GetProperty("communications")
                                       .GetProperty("edges");
            var hasNextPage = doc.RootElement.GetProperty("data")
                                            .GetProperty("communications")
                                            .GetProperty("pageInfo")
                                            .GetProperty("hasNextPage")
                                            .GetBoolean();

            Assert.Equal(2, edges.GetArrayLength());
            Assert.True(hasNextPage);
        }

        [Fact]
        public async Task Page2_UsingEndCursor_ReturnsNextItems()
        {
            var executor = await PaginationTestHelper.CreateExecutorAsync();
            // Get first page to retrieve endCursor
            var firstQuery = @"
                query {
                    communications(first: 2) {
                        pageInfo { endCursor hasNextPage }
                    }
                }";

            var firstResult = await executor.ExecuteAsync(firstQuery);
            using var firstDoc = JsonDocument.Parse(firstResult.ToJson());
            var endCursor = firstDoc.RootElement
                                    .GetProperty("data")
                                    .GetProperty("communications")
                                    .GetProperty("pageInfo")
                                    .GetProperty("endCursor")
                                    .GetString();

            // Query second page
            var secondQuery = $@"
                query {{
                    communications(first: 2, after: ""{endCursor}"") {{
                        edges {{ cursor node {{ id title }} }}
                        pageInfo {{ hasNextPage endCursor }}
                    }}
                }}";

            var secondResult = await executor.ExecuteAsync(secondQuery);
            secondResult.ToJson().MatchSnapshot();

            using var doc2 = JsonDocument.Parse(secondResult.ToJson());
            var edges2 = doc2.RootElement.GetProperty("data")
                                         .GetProperty("communications")
                                         .GetProperty("edges");

            Assert.NotEmpty(edges2.EnumerateArray());
        }

        [Fact]
        public async Task SnapshotOfResultShape()
        {
            var executor = await PaginationTestHelper.CreateExecutorAsync();
            var query = @"
                query {
                    communications(first: 2) {
                        edges { cursor node { id title } }
                        pageInfo { hasNextPage endCursor }
                    }
                }";

            var result = await executor.ExecuteAsync(query);
            result.ToJson().MatchSnapshot();
        }
    }
}
