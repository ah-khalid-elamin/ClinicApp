using ClinicApp.Models;

namespace ClinicApp.Services
{
    public interface PatientService
    {
        public List<Patient> GetPatients();
        public Patient GetPatient(int Id);
        public Patient Save(Patient patient);
        public Patient Update(Patient patient);
        public void Delete(int Id);

    }
}
