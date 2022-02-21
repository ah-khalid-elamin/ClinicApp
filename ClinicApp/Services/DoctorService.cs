using ClinicApp.Models;

namespace ClinicApp.Services
{
    public interface DoctorService
    {
        public List<Doctor> GetDoctors();
        public Doctor GetDoctor(int Id);
        public Doctor Save(Doctor doctor);
        public Doctor Update(Doctor doctor);
        public void Delete(int Id);


    }
}
