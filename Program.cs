using Kepas.Core.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Kepas.Core.Api.Domain.Interfaces;
using Kepas.Core.Api.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Database=KepasSystemDb;Username=postgres;Password=Kepas!StrongPassword123;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var redisConn = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConn;
    options.InstanceName = "KepasSystem_";
});

// Dependences
builder.Services.AddHttpContextAccessor();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IPlatformRoleService, PlatformRoleService>();
builder.Services.AddScoped<IPlatformAdminService, PlatformAdminService>();
builder.Services.AddScoped<IServiceAccountService, ServiceAccountService>();

// JWT Auth Config
var jwtKey = builder.Configuration["JwtSettings:Secret"] ?? "KEPAS_VERY_LONG_SECRET_KEY_FOR_JWT_SIGNATURE_12345!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add CORS Policy (Dynamic CORS will be implemented as a custom middleware later)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var supportedCultures = new[] { "pt-BR", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
    
app.UseRequestLocalization(localizationOptions);

// Inject Custom SaaS Middlewares
app.UseMiddleware<Kepas.Core.Api.Infrastructure.Middlewares.DynamicCorsMiddleware>();
app.UseMiddleware<Kepas.Core.Api.Infrastructure.Middlewares.TenantResolutionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.SeedData(context);
}

app.Run();
