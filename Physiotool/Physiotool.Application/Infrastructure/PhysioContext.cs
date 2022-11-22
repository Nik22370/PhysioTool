using Bogus;
using Microsoft.EntityFrameworkCore;
using Physiotool.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physiotool.Application.Infrastructure
{
    public class PhysioContext : DbContext
    {
        public DbSet<Patient> Patients => Set<Patient>();

        public static PhysioContext WithSqlite() => WithSqlite("physio.db");

        public static PhysioContext WithSqlite(string filename)
        {
            var opt = new DbContextOptionsBuilder<PhysioContext>()
                .UseSqlite($"Data Source={filename}")
                .Options;
            return new PhysioContext(opt);
        }

        public PhysioContext(DbContextOptions<PhysioContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1511);
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
            .Generate(20)
            .GroupBy(p => p.Email).Select(g => g.First())
            .ToList();
            Patients.AddRange(patients);
            SaveChanges();
        }
    }
}
