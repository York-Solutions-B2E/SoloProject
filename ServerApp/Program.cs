using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Okta.AspNetCore;
using ServerApp.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;


})
.AddCookie()
.AddOktaMvc(new OktaMvcOptions
{
    OktaDomain = builder.Configuration["Okta:OktaDomain"],
    ClientId = builder.Configuration["Okta:ClientId"],
    ClientSecret = builder.Configuration["Okta:ClientSecret"],
    AuthorizationServerId = builder.Configuration["Okta:AuthorizationServerId"],
    //to find groups role
    Scope = new List<string> { "openid", "profile", "email", "groups" },


});

/*
Configure Token validation Parameters
to accept groups in role based authorization.
*/
builder.Services.Configure<OpenIdConnectOptions>(OktaDefaults.MvcAuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "groups"
    };
});

builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    //Docker Compose name
    client.BaseAddress = new Uri("http://webapi"); 
})
.AddHttpMessageHandler<AccessTokenService>();


builder.Services.AddHttpContextAccessor();

//add login controller
builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//Sign on/roles Auth
app.UseAuthentication();
app.UseAuthorization();

//mapping login controller
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
