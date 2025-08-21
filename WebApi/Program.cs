using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.CommunicationDbContext;
using WebApi.Controllers;
using WebApi.Services;
using HotChocolate.AspNetCore;


var builder = WebApplication.CreateBuilder(args);


//Start controllers
builder.Services.AddControllers();

if (!builder.Environment.IsEnvironment("Test"))
{
    //Hook up to sqlserver
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    });

    // Add CORS policy to allow communication between API/Server
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins("http://serverapp") // frontend URLs
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
    builder.Services.AddScoped<ICommunicationRepository, CommunicationRepository>();
    builder.Services.AddScoped<ICommunicationTypeService, CommunicationTypeService>();
    builder.Services.AddScoped<ICommunicationService, CommunicationService>();
    builder.Services.AddScoped<CommunicationEventPublisher>();

    builder.Services.AddHostedService<CommunicationEventConsumer>();


}


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://integrator-7433132.okta.com/oauth2/default";
    options.Audience = "api://default";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "Roles"  // Important if you're using group-based roles
    };
});
builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddFiltering()   // <-- THIS IS REQUIRED if you use [UseFiltering]
    .AddProjections()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .DisableIntrospection(false)
    .AddSorting()
    .AddAuthorization();
    
    
var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();


// Development endpoint with IDE
if (app.Environment.IsDevelopment())
{
    // Dev-only endpoint with IDE enabled
    app.MapGraphQL("/graphql-dev")
       .WithOptions(new GraphQLServerOptions
       {
           Tool = { Enable = true }   // IDE enabled in dev
       });
}

// Production endpoint without IDE
app.MapGraphQL("/graphql")
   .WithOptions(new GraphQLServerOptions
   {
       Tool = { Enable = true }  // IDE disabled in prod
   });
   
app.UseCors("AllowSpecificOrigins");


app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }