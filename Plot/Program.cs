/*
    Filename: Program.cs
    Part of Project: PLOT/PLOT-FE/Plot

    Project Purpose: This project is the frontend for Plato's Closet
    PLOT floorset allocation software. It defines the main entry point for the application,
    sets up the services, authentication, and middleware for the Blazor application.

    Written by: SVSU 2025 Capstone Team
*/

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Plot.Data.Models.Env;
using System.Text;
using Plot.Components;
using Microsoft.IdentityModel.Tokens;
using Plot.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.JSInterop;
using System.Text.Json;
using PdfSharp.Fonts;

var builder = WebApplication.CreateBuilder(args);

// Add the environment settings from the .env file as a scoped service
// into the program service container.
EnvironmentSettings envSettings = new();
builder.Services.AddScoped<EnvironmentSettings>();

builder.WebHost.UseUrls(envSettings.audience);

// Add Interactive server components to the service container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// Add the services and database context classes into the program service container.
// These will be injected as dependencies throughout the program.
builder.Services.AddScoped<AuthHttpClient>();
builder.Services.AddScoped<FixturesHttpClient>();
builder.Services.AddScoped<FloorsetsHttpClient>();
builder.Services.AddScoped<StoresHttpClient>();
builder.Services.AddScoped<UsersHttpClient>();
builder.Services.AddScoped<SalesHttpClient>();
builder.Services.AddScoped<ICookie, Cookie>();
builder.Services.AddScoped<ClaimParserService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddSingleton<FloorsetEditorService>();

//Set fonts for the pdf
var fontPath = Path.Combine(builder.Environment.WebRootPath, "fonts", "segoeui.ttf");
if (!File.Exists(fontPath))
{
    throw new FileNotFoundException($"Font file not found: {fontPath}");
}

var fontBytes = File.ReadAllBytes(fontPath);
var customFontResolver = new CustomFontResolver(fontBytes);
GlobalFontSettings.FontResolver = customFontResolver;
builder.Services.AddSingleton<IFontResolver>(customFontResolver);


// Add the authentication middleware service into the program service container,
// adding Jwt bearer tokens into the program. The Jwt bearer token settings
// are based on the .env file values and seeing the implementation for the
// tokens.
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

            // Set auth cookies exp to the day before to delete it.
            context.Response.Cookies.Append("Auth", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
            });

            // Redirect to login
            context.Response.Redirect("/login");
            return Task.CompletedTask;
        }
    };
});


// Add the authorization middleware service into the program service container,
// allowing for policy authorization middleware. These will check the role
// claim tied to the auth token in the cookie for any Authorization policies 
// defined in the pages.
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
}) 
.AddHubOptions(options =>
{
    options.MaximumReceiveMessageSize = 1 * 1024 * 1024;
});;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(envSettings.issuer) 
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
// Middleware to redirect to the status code page
app.UseStatusCodePagesWithRedirects("/status-code/{0}");
app.Run();