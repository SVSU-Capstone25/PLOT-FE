using Microsoft.AspNetCore.Authentication.JwtBearer;
using Plot.Data.Models.Env;
using System.Text;
using Plot.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

EnvironmentSettings envSettings = new();
builder.Services.AddScoped<EnvironmentSettings>();

builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped(sp =>
{
    var handler = new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer(),
        AllowAutoRedirect = true
    };
    return new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.Configuration["BACKEND_URL"] ?? "http://backend:8085/api")
    };
});

builder.Services.AddScoped<AuthHttpClient>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = envSettings.issuer,
            ValidateAudience = true,
            ValidAudience = envSettings.audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(envSettings.secret_key)),
            ValidateLifetime = true,
        };
    });
     

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Employee", policy => policy.RequireClaim("Role", "3", "2", "1"))
    .AddPolicy("Manager", policy => policy.RequireClaim("Role", "1", "2"))
    .AddPolicy("Owner", policy => policy.RequireClaim("Role", "1"));
    
builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();



builder.Services.AddAuthorizationCore();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
// Registers IAuthenticationService
app.UseAuthorization();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
