using Common.Models;
using Common.Wrappers;

namespace Common.Services
{
    public interface DoctorService
    {
        public List<Doctor> GetDoctors();
        public Doctor GetDoctor(int Id);
        public Doctor Save(Doctor doctor);
        public Doctor Update(int Id, Doctor doctor);
        public void Delete(int Id);
        public List<Appointment> GetAllDoctorAppointments(int id);
        public List<Appointment> GetAllDoctorAppointmentsByDay(int id, DateTime Date);
        public bool IsAvailableforAnAppointmentByDate(int id, DateTime Date);
        public List<Doctor> GetDoctorsWithMostAppointmentsByDate(DateTime Date);
        public List<Doctor> GetDoctorsWithAppointmentsExceedingSixHoursByDate(DateTime Date);
        public List<Slot> GetDoctorAvailableSlots(int id,DateTime date);
        public List<String> ExportDoctorsToCsv();
    }
}
