using AccessControl.Api.Data;
using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Lib.ExceptionHandling;
using Shared.Lib.Extensions;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

// For Entity Framework
if (configuration["Settings:UseInMemoryDb"] == "true")
{
    //InMemory Db Conf
    builder.Services.AddDbContext<ClayDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "ClayDb"));
}
else
{
    //SQL DB Conf
    builder.Services.AddDbContext<ClayDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ClayDbConnectionString")));
}

//Add Repositories
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<ILocksRepository, LocksRepository>();
builder.Services.AddScoped<IAccessorsRepository, AccessorsRepository>();
builder.Services.AddScoped<IAccessesRepository, AccessesRepository>();
builder.Services.AddScoped<ILocksAccessorsRepository, LocksAccessorsRepository>();

// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ClayDbContext>()
    .AddDefaultTokenProviders();

//Add Api Version
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});

builder.Services.AddVersionedApiExplorer(
                options => options.GroupNameFormat = "'v'VVV");

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

// Add Controllers Config
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(
            "https://localhost:6052",
            "http://localhost:5002")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

//Seed Db
app.MigrateDatabase<ClayDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<ClayDbContextSeed>>();
    ClayDbContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();


// global error handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

public partial class AccessControlApiProgram { }