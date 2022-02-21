using ClinicApp.DbContexts;
using ClinicApp.Models;

namespace ClinicApp.Services.Impl
{
    public class PatientServiceImpl : PatientService
    {
        public PatientDBContext PatientDBContext { get; set; }

        public PatientServiceImpl(PatientDBContext _patientDBContext)
        {
            PatientDBContext = _patientDBContext;
        }
        public List<Patient> GetPatients()
        {
            return PatientDBContext.Patients.ToList();
        }
        public Patient GetPatient(int Id)
        {
            return PatientDBContext.Patients.ToList().FirstOrDefault(c => c.Id == Id);
        }
        public Patient Save(Patient patient)
        {
            PatientDBContext.Patients.Add(patient);
            PatientDBContext.SaveChanges();
            return patient;
        }
        public Patient Update(Patient patient)
        {
            PatientDBContext.Patients.Update(patient);
            PatientDBContext.SaveChanges();
            return patient;
        }
        public void Delete(int Id)
        {
            Patient patient = GetPatient(Id);
            PatientDBContext.Patients.Remove(patient);
            PatientDBContext.SaveChanges();
        }

    }
}
