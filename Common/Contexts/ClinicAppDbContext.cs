using Microsoft.EntityFrameworkCore;
using Common.Models;

namespace Common.Contexts
{
    public class ClinicAppDbContext : DbContext
    {
        public ClinicAppDbContext(DbContextOptions<ClinicAppDbContext> options)
                   : base(options)
        {
        }
        public ClinicAppDbContext() { }
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
    }
}
