using ClinicApp.DbContexts;
using ClinicApp.Models;

namespace ClinicApp.Services.Impl
{
    public class DoctorServiceImpl : DoctorService
    {
        public DoctorDBContext DoctorDBContext { get; set; }

        public DoctorServiceImpl(DoctorDBContext _patientDBContext)
        {
            DoctorDBContext = _patientDBContext;
        }
        public List<Doctor> GetDoctors()
        {
            return DoctorDBContext.Doctors.ToList();
        }
        public Doctor GetDoctor(int Id)
        {
            return DoctorDBContext?.Doctors.ToList().FirstOrDefault(c => c.Id == Id);
        }
        public Doctor Save(Doctor doctor)
        {
            DoctorDBContext.Doctors.Add(doctor);
            DoctorDBContext.SaveChanges();
            return doctor;
        }
        public Doctor Update(Doctor doctor)
        {
            DoctorDBContext.Doctors.Update(doctor);
            DoctorDBContext.SaveChanges();
            return doctor;
        }
        public void Delete(int Id)
        {
            Doctor doctor = GetDoctor(Id);
            DoctorDBContext.Doctors.Remove(doctor);
            DoctorDBContext.SaveChanges();
        }
    }
}
