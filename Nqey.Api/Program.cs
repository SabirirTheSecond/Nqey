using Microsoft.EntityFrameworkCore;
using Nqey.DAL;
using AutoMapper;
using Nqey.DAL.Repositories;
using Nqey.Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Identity;
using Nqey.Domain.Abstractions.Services;
using Nqey.Services.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(connectionString));

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>();

builder.Services.AddAuthentication();


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
