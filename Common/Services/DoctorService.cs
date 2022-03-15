using Common.Models;
using Common.Wrappers;

namespace Common.Services
{
    public interface DoctorService
    {
        public List<Doctor> GetDoctors();
        public Doctor GetDoctor(string Id);
        public Doctor Save(Doctor doctor);
        public Doctor Update(string Id, Doctor doctor);
        public void Delete(string Id);
        public List<Appointment> GetAllDoctorAppointments(string id);
        public List<Appointment> GetAllDoctorAppointmentsByDay(string id, DateTime Date);
        public bool IsAvailableforAnAppointmentByDate(string id, DateTime Date);
        public List<Doctor> GetDoctorsWithMostAppointmentsByDate(DateTime Date);
        public List<Doctor> GetDoctorsWithAppointmentsExceedingSixHoursByDate(DateTime Date);
        public List<Slot> GetDoctorAvailableSlots(string id,DateTime date);
        public List<String> ExportDoctorsToCsv();
    }
}
