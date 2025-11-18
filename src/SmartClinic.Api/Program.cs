using Microsoft.EntityFrameworkCore;
using SmartClinic.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using SmartClinic.Api.Models;
using SmartClinic.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// -------------------- Validate critical settings --------------------
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
var blobConn = builder.Configuration["BlobStorage:ConnectionString"];
var blobContainer = builder.Configuration["BlobStorage:ContainerName"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
    throw new Exception("JWT settings missing. Set Jwt__Key, Jwt__Issuer, Jwt__Audience in Azure App Settings.");

if (string.IsNullOrWhiteSpace(connStr))
    throw new Exception("DefaultConnection missing. Set ConnectionStrings__DefaultConnection in Azure App Settings.");

if (string.IsNullOrWhiteSpace(blobConn) || string.IsNullOrWhiteSpace(blobContainer))
    throw new Exception("BlobStorage settings missing. Set BlobStorage__ConnectionString and BlobStorage__ContainerName in Azure App Settings.");

// -------------------- Services --------------------
builder.Services.AddDbContext<ClinicContext>(options =>
    options.UseSqlServer(connStr));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ClinicContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddControllers();

// -------------------- Swagger with JWT --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartClinic API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<BlobService>();

var app = builder.Build();

// -------------------- Middleware --------------------
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// -------------------- Seed Roles --------------------
await SeedRolesAsync(app);

app.Run();

static async Task SeedRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Doctor", "Receptionist" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
