
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;

namespace ClinicApp.DbContexts
{
    public class PatientDBContext : DbContext
    {
        public PatientDBContext(DbContextOptions<PatientDBContext> options)
                    : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; } = null!;
    }
}
