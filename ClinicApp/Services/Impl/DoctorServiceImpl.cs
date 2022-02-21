using ClinicApp.DbContexts;
using ClinicApp.Models;

namespace ClinicApp.Services.Impl
{
    public class DoctorServiceImpl : DoctorService
    {
        public ClinicAppDbContext ClinicAppDbContext { get; set; }

        public DoctorServiceImpl(ClinicAppDbContext clinicAppDbContext)
        {
            ClinicAppDbContext = clinicAppDbContext;
        }
        public List<Doctor> GetDoctors()
        {
            return ClinicAppDbContext.Doctors.ToList();
        }
        public Doctor GetDoctor(int Id)
        {
            return ClinicAppDbContext?.Doctors.ToList().FirstOrDefault(c => c.Id == Id);
        }
        public Doctor Save(Doctor doctor)
        {
            ClinicAppDbContext.Doctors.Add(doctor);
            ClinicAppDbContext.SaveChanges();
            return doctor;
        }
        public Doctor Update(Doctor doctor)
        {
            ClinicAppDbContext.Doctors.Update(doctor);
            ClinicAppDbContext.SaveChanges();
            return doctor;
        }
        public void Delete(int Id)
        {
            Doctor doctor = GetDoctor(Id);
            ClinicAppDbContext.Doctors.Remove(doctor);
            ClinicAppDbContext.SaveChanges();
        }

        public List<Appointment> GetAllDoctorAppointments(int id)
        {
            return ClinicAppDbContext.Appointments.Where(appointment => appointment.Doctor.Id == id).ToList();
        }

    }
}
