using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Physiotool.Application.Infrastructure;
using Physiotool.Application.Services;
using Physiotool.Webapi.Controllers;

// Scaffolding the Database
using (var db = PhysioContext.WithSqlite())
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed();
}

// Liefert alle Tage dieses Jahrhunderts mit den österreichischen Feiertagen.
var calendarService = new CalendarService(2000, 2100);

// *************************************************************************************************

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });
}

// *************************************************************************************************

var app = builder.Build();
// Für den vite Dev Server. Er greift von einem anderen Port auf die API zu.
if (app.Environment.IsDevelopment()) { app.UseCors(); }
app.UseStaticFiles();

// Liefert alle Patienten
// Beispiel: http://localhost:5000/api/patients
app.MapGet("/api/patients", () => new PatientController(PhysioContext.WithSqlite()).GetAllPatients());

// Liefert die Termine eines Monats.
// Beispiel: http://localhost:5000/api/calendar/2022/1
app.MapGet("/api/calendar/{year:int}/{month:int}", (int year, int month) =>
    new CalendarController(PhysioContext.WithSqlServerContainer(), calendarService).GetCalendar(year, month));

app.MapFallbackToFile("index.html");
app.Run();
