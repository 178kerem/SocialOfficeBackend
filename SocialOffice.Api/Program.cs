using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialOffice.Application.Extensions;
using SocialOffice.Infrastructure.Extensions;
using SocialOffice.Infrastructure.Persistence;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // JSON cycle problemi çözümü

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // frontend adresin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

// EF Core - PostgreSQL
builder.Services.AddDbContext<SOfficeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SosyalOfis Api",
        Version = "v1",
        Description = "SosyalOfis projesi için RESTful API dokümantasyonu"
    });

    // XML dokümantasyon varsa ekle
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Custom Service Registrations
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureService();

var app = builder.Build();

// HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SosyalOfis API V1");
        options.RoutePrefix = string.Empty; // Swagger ana sayfada açýlýr
    });
}

app.UseHttpsRedirection();
app.UseCors(); // CORS middleware aktif ediliyor
app.UseAuthentication(); // Authentication kullanýyorsan
app.UseAuthorization();
app.MapControllers();
app.Run();
