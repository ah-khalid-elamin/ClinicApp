using Common.Contexts;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Services.Impl
{
    public class PatientServiceImpl : PatientService
    {
        public ClinicAppDbContext ClinicAppDbContext { get; set; }

        public PatientServiceImpl(ClinicAppDbContext clinicAppDbContext)
        {
            this.ClinicAppDbContext = clinicAppDbContext;
        }
        public List<Patient> GetPatients()
        {
            return ClinicAppDbContext.Patients.ToList();
        }
        public Patient GetPatient(string Id)
        {
            return ClinicAppDbContext.Patients.ToList().FirstOrDefault(c => c.Id == Id);
        }
        public Patient Save(Patient patient)
        {
            ClinicAppDbContext.Patients.Add(patient);
            ClinicAppDbContext.SaveChanges();
            return patient;
        }
        public Patient Update(string id, Patient patient)
        {
            Patient _existingPatient = GetPatient(id);
            if (_existingPatient != null)
            {
                ClinicAppDbContext.Patients.Update(patient);
                ClinicAppDbContext.SaveChanges();

            }
            else
            {
                throw new Exception("No Such Patient Exist");
                return null;
            }

            return patient;
        }
        public void Delete(string Id)
        {
            Patient patient = GetPatient(Id);
            ClinicAppDbContext.Patients.Remove(patient);
            ClinicAppDbContext.SaveChanges();
        }

        public List<Appointment> GetPatientPreviousAppointments(string patientId)
        {
           return ClinicAppDbContext.Appointments.Include(a => a.Patient).AsNoTracking().AsEnumerable()
                .Where(appointment => appointment?.Patient?.Id == patientId)
                .OrderByDescending(k=> k.StartDate).ToList();
        }

        public List<string> ExportPatientsToCsv()
        {
            List<String> results = new List<string>();

            List<Patient> patients = GetPatients();
            foreach (Patient patient in patients)
            {
                results.Add(patient.ToString());
            }

            return results;

        }

    }
}
