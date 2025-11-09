using Microsoft.EntityFrameworkCore;
using SmartClinic.Api.Models;

namespace SmartClinic.Api.Data
{
    public class ClinicContext : DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
    }
}