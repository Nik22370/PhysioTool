using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Physiotool.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physiotool.Application.Infrastructure
{
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
            : base(v => v.ToDateTime(new TimeOnly(0)), v => DateOnly.FromDateTime(v))
        { }
    }

    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter()
            : base(v => v.ToTimeSpan(), v => TimeOnly.FromTimeSpan(v))
        { }
    }

    public class PhysioContext : DbContext
    {
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        // Wir geben den Subtype nicht nach außen, brauchen aber die Registrierung damit er
        // erkannt wird.
        protected DbSet<ConfirmedAppointment> ConfirmedAppointments => Set<ConfirmedAppointment>();
        // Wir geben den Subtype nicht nach außen, brauchen aber die Registrierung damit er
        // erkannt wird.
        protected DbSet<DeletedAppointment> DeletedAppointments => Set<DeletedAppointment>();

        public static PhysioContext WithSqlite() => WithSqlite("physio.db");
        public static PhysioContext WithSqlServerContainer()
        {
            var opt = new DbContextOptionsBuilder<PhysioContext>()
                .UseSqlServer(@"Server=127.0.0.1,1433;Initial Catalog=PhysioDb;User Id=sa;Password=SqlServer2019")
                .Options;
            return new PhysioContext(opt);
        }

        public static PhysioContext WithSqlite(string filename)
        {
            var opt = new DbContextOptionsBuilder<PhysioContext>()
                .UseSqlite($"Data Source={filename}")
                .Options;
            return new PhysioContext(opt);
        }

        public PhysioContext(DbContextOptions<PhysioContext> opt) : base(opt)
        { }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>();
            configurationBuilder.Properties<TimeOnly>().HaveConversion<TimeOnlyConverter>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1511);

            // Demopatienten erstellen
            var patients = new Faker<Patient>("de").CustomInstantiator(f =>
            {
                return new Patient(
                    firstname: f.Name.FirstName(),
                    lastname: f.Name.LastName(),
                    street: f.Address.StreetAddress(),
                    zip: f.Random.Int(100, 999) * 10,
                    city: f.Address.City(),
                    email: f.Internet.Email(),
                    phone: f.Phone.PhoneNumber());
            })
            .Generate(200)
            .GroupBy(p => p.Email).Select(g => g.First())
            .ToList();
            Patients.AddRange(patients);
            SaveChanges();

            // Termine zu den Patienten erstellen. Dafür erstellen wir 100 Termine und weisen sie
            // jeweils einen zufälligen Patienten zu.
            var appointments = new Faker<Appointment>("de").CustomInstantiator(f =>
            {
                // Ein Termin wird zwischen 1.9.2020 und 1.6.2022 erstellt.
                var date = f.Date.BetweenDateOnly(new DateOnly(2020, 9, 1), new DateOnly(2022, 6, 1));
                // Termine nur zu vollen 1/4 Stunden erstellen.
                var time = new TimeOnly(8, 0).AddMinutes(f.Random.Int(0, 32) * 15);
                // 1 - 3 Tage vor dem Termin ist die Anmeldung.
                var appointmentCreated = date.ToDateTime(time).AddSeconds(-f.Random.Int(24 * 3600, 72 * 3600));
                return f.Random.Int(1, 3) switch
                {
                    2 => new ConfirmedAppointment(
                        date: date, time: time, patient: f.Random.ListItem(patients), created: appointmentCreated,
                        duration: TimeSpan.FromMinutes(f.Random.Int(2, 8) * 15)),
                    3 => new DeletedAppointment(date: date, time: time, patient: f.Random.ListItem(patients), created: appointmentCreated),
                    _ => new Appointment(date: date, time: time, patient: f.Random.ListItem(patients), created: appointmentCreated)
                };
            })
            .Generate(1000)
            .ToList();

            Appointments.AddRange(appointments);
            SaveChanges();
        }
    }
}
