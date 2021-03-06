using Common.Models;

namespace Common.Services
{
    public interface AppointmentService
    {
        public List<Appointment> GetAllAppointments();
        public Appointment GetAppointmentDetails(int id);
        public Appointment BookAnAppointment(Doctor doctor,Patient patient,DateTime StartDate,DateTime EndDate,bool confirmed);
        public Appointment BookAnAppointment(Appointment appointment);
        public Appointment UpdateAnAppointment(int id, Appointment appointment);
        public void CancelAppointment(int Id);
        List<string> ExportAppointmentsToCsv();
    }
}
