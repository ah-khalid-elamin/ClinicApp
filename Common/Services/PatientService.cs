using Common.Models;

namespace Common.Services
{
    public interface PatientService
    {
        public List<Patient> GetPatients();
        public Patient GetPatient(string Id);
        public Patient Save(Patient patient);
        public Patient Update(string id, Patient patient);
        public void Delete(string Id);
        public List<Appointment> GetPatientPreviousAppointments(string patientId);
        List<string> ExportPatientsToCsv();
    }
}
