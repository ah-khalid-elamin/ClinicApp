using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;

namespace ClinicApp.DbContexts
{
    public class ClinicAppDbContext : DbContext
    {
        public ClinicAppDbContext(DbContextOptions<ClinicAppDbContext> options)
                   : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
    }
}
