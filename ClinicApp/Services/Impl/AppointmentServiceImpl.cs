using ClinicApp.DbContexts;
using ClinicApp.Models;

namespace ClinicApp.Services.Impl
{
    public class AppointmentServiceImpl : AppointmentService
    {
        public ClinicAppDbContext ClinicAppDbContext { get; set; }

        public AppointmentServiceImpl(ClinicAppDbContext clinicAppDbContext)
        {
            ClinicAppDbContext = clinicAppDbContext;
        }

        public Appointment BookAnAppointment(Doctor doctor, Patient patient, DateTime StartDate, DateTime EndDate, bool confirmed)
        {
            // check doctor availability
            throw new NotImplementedException();
        }

        public Appointment BookAnAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public void CancelAppointment(int Id)
        {
            Appointment appointment = GetAppointmentDetails(Id);
            ClinicAppDbContext.Appointments.Remove(appointment);
            ClinicAppDbContext.SaveChanges();

        }

        public List<Appointment> GetAllAppointments()
        {
            return ClinicAppDbContext.Appointments.ToList();
        }

        public Appointment GetAppointmentDetails(int id)
        {
            return ClinicAppDbContext?.Appointments?.Where(appointment => appointment.Id == id).FirstOrDefault();
        }

        public Appointment UpdateAnAppointment(int id, Appointment appointment)
        {
            Appointment existingAppointment = GetAppointmentDetails(id);
            if (existingAppointment != null) throw new InvalidOperationException("No Such Appointment to be updated");

            ClinicAppDbContext.Appointments.Update(appointment);
            ClinicAppDbContext.SaveChanges();

            return appointment;
        }
    }
}
