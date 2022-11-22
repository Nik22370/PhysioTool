using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Physiotool.Application.Infrastructure;
using Physiotool.Webapi.Controllers;

// Scaffolding the Database
using (var db = PhysioContext.WithSqlite())
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed();
}


var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}
app.UseStaticFiles();

// Liefert alle News mit ID, BildURL, Headline
app.MapGet("/api/patients", () => new PatientController(PhysioContext.WithSqlite()).GetAllPatients());

app.MapFallbackToFile("index.html");
app.Run();
