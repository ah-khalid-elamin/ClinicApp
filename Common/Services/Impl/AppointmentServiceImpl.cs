using Common.Contexts;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Services.Impl
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
            
            //check clinic working hours
            DateTime clinicStart = DateTime.Parse($"{StartDate.ToShortDateString()} 9:00");
            DateTime clinicEnd = DateTime.Parse($"{StartDate.ToShortDateString()} 21:00");

            if(StartDate < clinicStart)
                throw new InvalidOperationException("Clinic opens at 9:00 AM");

            if (EndDate > clinicEnd)
                throw new InvalidOperationException("Clinic closes at 9:00 PM");

            if (doctor == null) throw new ArgumentNullException("Doctor is missing");
            if (patient == null) throw new ArgumentNullException("Patient is missing");

            //check appointment duration
            if (!checkAppointmentDuration(StartDate,EndDate))
            {
                throw new InvalidOperationException("There is an error with the appointment duration");
            }

            // check doctor availability
            if (!doctorService.IsAvailableforAnAppointmentByDate(doctor.Id, StartDate))
                throw new InvalidOperationException("This doctor is full today.");

            if (!checkIsAvailableSlot(doctor.Id,StartDate,EndDate))
                throw new InvalidOperationException("This Appointment schedule conflict with other appointments");

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
            return clinicAppDbContext.Appointments.Include(a=>a.Doctor).Include(a=>a.Patient).AsNoTracking().ToList();
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
        private bool checkIsAvailableSlot(int doctorId,DateTime targetStart, DateTime targetEnd)
        {
            List<Appointment> appointments = doctorService.GetAllDoctorAppointmentsByDay(doctorId,targetStart);

            //check targetStart is not between other appointments start and end datetime
            foreach (var appointment in appointments)
            {
                if (ExistBetween(appointment.StartDate, appointment.EndDate, targetStart))
                    return false;
            }
            //check targetEnd is not between other appointments start and end datetime
            foreach (var appointment in appointments)
            {
                if (ExistBetween(appointment.StartDate, appointment.EndDate, targetEnd))
                    return false;
            }

            return true;
        }
        public bool ExistBetween(DateTime start, DateTime End, DateTime target)
        {
            if(start.CompareTo(target) < 0 && End.CompareTo(target) > 0)
                return true;

            return false;
        }

        public List<string> ExportAppointmentsToCsv()
        {
            List<String> results = new List<string>();

            List<Appointment> appointments = GetAllAppointments();
            foreach (Appointment appointment in appointments)
            {
                results.Add(appointment.ToString());
            }

            return results;
        }
    }
}
