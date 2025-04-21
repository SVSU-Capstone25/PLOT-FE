using Microsoft.AspNetCore.Authentication.JwtBearer;
using Plot.Data.Models.Env;
using System.Text;
using Plot.Components;
using Microsoft.IdentityModel.Tokens;
using Plot.Services;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Get environment settings
EnvironmentSettings envSettings = new();

// Add service to the container to get env settings in the app.
builder.Services.AddScoped<EnvironmentSettings>();

builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// Add HttpClient for server-side Blazor
// builder.Services.AddScoped(sp =>
// {
//     var handler = new HttpClientHandler
//     {   //Allows manipulating cookies in the request
//         UseCookies = false,
//         AllowAutoRedirect = true
//     };
//     return new HttpClient(handler)
//     {
//         BaseAddress = new Uri(builder.Configuration["BACKEND_URL"] ?? "http://backend:8085/api")
//     };
// });

// Add services for authentication and authorization
builder.Services.AddScoped<AuthService>();
// builder.Services.AddScoped<AuthHeaderHttpClient>();
builder.Services.AddScoped<AuthHttpClient>();
builder.Services.AddScoped<FixturesHttpClient>();
builder.Services.AddScoped<FloorsetsHttpClient>();
builder.Services.AddScoped<StoresHttpClient>();
builder.Services.AddScoped<UsersHttpClient>();
builder.Services.AddScoped<SalesHttpClient>();
builder.Services.AddScoped<ICookie, Cookie>();
builder.Services.AddScoped<JwtService>();




// Add JWT authentication and authorization 
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // Configure JWT authentication
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = envSettings.issuer,
        ValidateAudience = true,
        ValidAudience = envSettings.audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(envSettings.auth_secret_key)),
        ValidateLifetime = true,
    };

    // Configure JWT bearer events
    options.Events = new JwtBearerEvents
    {
        // This lets the http connection in the underlying signalr connection
        // able to verify off of jwt token from the cookie
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["Auth"];
            context.Token = token;
            return Task.CompletedTask;
        },
        // Handle authentication failure and redirect to login page
        OnChallenge = context =>
        {
            // Stop the default behavior
            context.HandleResponse();

            // Return 401 or redirect to frontend login page
            context.Response.StatusCode = 401; // Unauthorized

             // Redirect to login
            context.Response.Redirect("/login");
            return Task.CompletedTask;
        }
    };
});

// Add authorization policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Employee", policy => policy.RequireClaim("Role", "Owner", "Manager", "Employee"))
    .AddPolicy("Manager", policy => policy.RequireClaim("Role", "Owner", "Manager"))
    .AddPolicy("Owner", policy => policy.RequireClaim("Role", "Owner"));

builder.Services.AddCascadingAuthenticationState();


builder.Services.AddAuthorizationCore();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://backend:8085", "http://localhost:8085") // Add your actual frontend URL(s)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
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
// Setup authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings = {
            [".js"] = "application/javascript"
        }
    }
});

app.UseCors(MyAllowSpecificOrigins);


app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseStatusCodePagesWithRedirects("/status-code/{0}");
app.Run();
