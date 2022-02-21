using ClinicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.DbContexts
{
    public class DoctorDBContext : DbContext
    {
        public DoctorDBContext(DbContextOptions<DoctorDBContext> options)
                  : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; } = null!;
    }
}
