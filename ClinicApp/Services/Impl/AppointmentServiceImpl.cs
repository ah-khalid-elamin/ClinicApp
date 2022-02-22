using ClinicApp.DbContexts;
using ClinicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Services.Impl
{
    public class AppointmentServiceImpl : AppointmentService
    {
        private readonly ClinicAppDbContext clinicAppDbContext;
        private readonly DoctorService doctorService;

        public AppointmentServiceImpl(ClinicAppDbContext clinicAppDbContext,DoctorService doctorService)
        {
            this.clinicAppDbContext = clinicAppDbContext;
            this.doctorService = doctorService;
        }

        public Appointment BookAnAppointment(Doctor doctor, Patient patient, DateTime StartDate, DateTime EndDate, bool confirmed)
        {
            // check doctor availability
            if (!doctorService.IsAvailableforAnAppointmentByDate(doctor.Id, StartDate))
                throw new Exception("This doctor is full today.");

            if (!checkAppointmentDuration(StartDate,EndDate))
            {
                throw new Exception("This is an error with the appointment duarion");
            }

            Appointment appointment = new Appointment()
            {
                Doctor = doctor,
                Patient = patient,
                StartDate = StartDate,
                EndDate = EndDate
            };
            clinicAppDbContext.Entry(appointment.Doctor).State = EntityState.Unchanged;
            clinicAppDbContext.Entry(appointment.Patient).State = EntityState.Unchanged;
            clinicAppDbContext.Appointments.Add(appointment);
            clinicAppDbContext.SaveChanges();

            return appointment;
        }

        public Appointment BookAnAppointment(Appointment appointment)
        {
            return BookAnAppointment(appointment.Doctor, appointment.Patient, appointment.StartDate, appointment.EndDate, true);
        }

        public void CancelAppointment(int Id)
        {
            Appointment appointment = GetAppointmentDetails(Id);
            clinicAppDbContext.Appointments.Remove(appointment);
            clinicAppDbContext.SaveChanges();

        }

        public List<Appointment> GetAllAppointments()
        {
            return clinicAppDbContext.Appointments.ToList();
        }

        public Appointment GetAppointmentDetails(int id)
        {
            return clinicAppDbContext?.Appointments?.Where(appointment => appointment.Id == id).FirstOrDefault();
        }
        public Appointment UpdateAnAppointment(int id, Appointment appointment)
        {
            Appointment existingAppointment = GetAppointmentDetails(id);
            if (existingAppointment != null) throw new InvalidOperationException("No Such Appointment to be updated");

            clinicAppDbContext.Appointments.Update(appointment);
            clinicAppDbContext.SaveChanges();

            return appointment;
        }
        private bool checkAppointmentDuration(DateTime StartDate, DateTime EndDate)
        {
            TimeSpan duration = (EndDate - StartDate);

            if (duration.TotalMinutes < 15)
                return false;
            if (duration.TotalHours > 2)
            
                return false;

            return true;
        }
    }
}
