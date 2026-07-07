using EsemenyMenedzser.BLL.CQRS;
using EsemenyMenedzser.BLL.CQRS.Interfaces;
using EsemenyMenedzser.BLL.Modul.Esemeny.Commands;
using EsemenyMenedzser.BLL.Modul.Esemeny.Queries;
using EsemenyMenedzser.BLL.Modul.Esemeny.Validators;
using EsemenyMenedzser.BLL.Services;
using EsemenyMenedzser.BLL.Services.Interfaces;
using EsemenyMenedzser.DAL;
using EsemenyMenedzser.DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICQRSExecutor, CQRSExecutor>();

builder.Services.AddScoped<IQueryHandler<GetEsemenyekListQuery, List<Esemeny>>, GetEsemenyekListQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetEsemenyByIdQuery, Esemeny?>, GetEsemenyByIdQueryHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateEsemenyCommandValidator>();
builder.Services.AddScoped<ICommandHandler<CreateEsemenyCommand, int>, CreateEsemenyCommandHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<UpdateEsemenyCommandValidator>();
builder.Services.AddScoped<ICommandHandler<UpdateEsemenyCommand, bool>, UpdateEsemenyCommandHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<DeleteEsemenyCommandValidator>();
builder.Services.AddScoped<ICommandHandler<DeleteEsemenyCommand, bool>, DeleteEsemenyCommandHandler>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
