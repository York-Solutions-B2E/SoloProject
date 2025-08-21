using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;


namespace WebApi.IntegrationTests
{
    public class GraphQLTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GraphQLTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UnauthorizedQuery_ShouldReturnAuthNotAuthenticated()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
            }).CreateClient();

            var query = new { query = "{ secret }" };
            var response = await client.PostAsJsonAsync("/graphql", query);

            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("AUTH_NOT_AUTHENTICATED", json);
        }

        [Fact]
        public async Task AuthorizedHealthQuery_ShouldReturn200()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication("Test")
                            .AddScheme<TestAuthOptions, TestAuthHandler>("Test", _ => { });
                });
            }).CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "stub-token");

            var query = new { query = "{ health }" };
            var response = await client.PostAsJsonAsync("/graphql", query);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SchemaIntrospection_ShouldIncludeEntryPoints()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
            }).CreateClient();

            var introspectionQuery = new
            {
                query = @"{ __schema { queryType { name } mutationType { name } subscriptionType { name } } }"
            };

            var response = await client.PostAsJsonAsync("/graphql", introspectionQuery);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Query", content);
            Assert.Contains("Mutation", content);
            Assert.Contains("Subscription", content);
        }

        


    }

}
