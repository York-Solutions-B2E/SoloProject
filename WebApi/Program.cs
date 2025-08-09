using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.CommunicationDbContext;
using WebApi.Services;


var builder = WebApplication.CreateBuilder(args);

//Start controllers
builder.Services.AddControllers();


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

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

