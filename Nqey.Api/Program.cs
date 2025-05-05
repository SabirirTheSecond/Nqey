using Microsoft.EntityFrameworkCore;
using Nqey.DAL;
using AutoMapper;
using Nqey.DAL.Repositories;
using Nqey.Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Identity;
using Nqey.Domain.Abstractions.Services;
using Nqey.Services.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Nqey.Domain.Common;

using Microsoft.OpenApi.Models;
using Nqey.Domain;
using Nqey.Domain.Authorization;
using Microsoft.AspNetCore.Authorization;
using Nqey.Services.Authorization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Nqey API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT like: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});



builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthorizationHandler, ActiveAccountHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsOwnerHandler>();


// Add services to the container.

// 1 load jwtSettings from config 
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudiences = jwtSettings.Audiences,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActiveAccountOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new ActiveAccountRequirement());
    });

    options.AddPolicy("IsOwner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new IsOwnerRequirement());
    });
}
);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            //.WithOrigins("nqey-production.up.railway.app")
            .AllowAnyOrigin() // Or use .WithOrigins("https://yourdomain.com") for security
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nqey API v1")
    );

}


app.UseRouting();
//if (!app.Environment.IsDevelopment())
//    app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
