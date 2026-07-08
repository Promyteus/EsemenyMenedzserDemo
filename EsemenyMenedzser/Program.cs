using EsemenyMenedzser.BLL.CQRS;
using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.BLL.Modul.Esemeny.Queries;
using EsemenyMenedzser.BLL.Modul.Esemeny.Validators;
using EsemenyMenedzser.BLL.Services;
using EsemenyMenedzser.BLL.Services.Interfaces;
using EsemenyMenedzser.DAL;
using EsemenyMenedzser.DAL.Entities;
using EsemenyMenedzser.DAL.Seed;
using EsemenyMenedzser.Exceptions;
using EsemenyMenedzser.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- LOGGING CONFIGURATION ---
var logsPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFileLogger(logsPath, (category, logLevel) =>
{
    return logLevel >= LogLevel.Information;
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- IDENTITY MODOSÍTÁS ---
// Ha tisztán cookie-t szeretnél API endpoints helyett, a hagyományos AddIdentity-t érdemes használni:
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cooki configuration to local development and CORS
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICQRSExecutor, CQRSExecutor>();
builder.Services.AddScoped<IQueryHandler<GetEventListQuery, List<Event>>, GetEventListQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetEventByIdQuery, Event?>, GetEventByIdQueryHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEventCommandValidator>();
builder.Services.AddScoped<ICommandHandler<CreateEventCommand, int>, CreateEventCommandHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEventCommandValidator>();
builder.Services.AddScoped<ICommandHandler<UpdateEventCommand, bool>, UpdateEventCommandHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteEventCommandValidator>();
builder.Services.AddScoped<ICommandHandler<DeleteEventCommand, bool>, DeleteEventCommandHandler>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithExposedHeaders("Content-Type", "Authorization");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// --- EXPLICIT PREFLIGHT HANDLER (OPTIONS) ---
app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        var origin = context.Request.Headers["Origin"].ToString();
        if (origin.Contains("localhost:4200"))
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
            context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Append("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"].ToString());
            context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        }
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});

app.UseExceptionHandler();

app.UseRouting();

app.UseCors("AngularPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- APPLY MIGRATIONS START ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Applying pending migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while applying migrations.");
        throw;
    }
}
// --- APPLY MIGRATIONS END ---

// --- SEEDING START ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await IdentityDataSeeder.SeedTechnicalUserAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the technical user.");
    }
}
// --- SEEDING END ---

app.Run();