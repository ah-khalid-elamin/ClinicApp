using ClinicApp.DbContexts;
using ClinicApp.Models;

namespace ClinicApp.Services.Impl
{
    public class DoctorServiceImpl : DoctorService
    {
        public ClinicAppDbContext ClinicAppDbContext { get; set; }

        public DoctorServiceImpl(ClinicAppDbContext clinicAppDbContext)
        {
            ClinicAppDbContext = clinicAppDbContext;
        }
        public List<Doctor> GetDoctors()
        {
            return ClinicAppDbContext.Doctors.ToList();
        }
        public Doctor GetDoctor(int Id)
        {
            return ClinicAppDbContext?.Doctors.ToList().FirstOrDefault(c => c.Id == Id);
        }
        public Doctor Save(Doctor doctor)
        {
            ClinicAppDbContext.Doctors.Add(doctor);
            ClinicAppDbContext.SaveChanges();
            return doctor;
        }
        public Doctor Update(Doctor doctor)
        {
            ClinicAppDbContext.Doctors.Update(doctor);
            ClinicAppDbContext.SaveChanges();
            return doctor;
        }
        public void Delete(int Id)
        {
            Doctor doctor = GetDoctor(Id);
            ClinicAppDbContext.Doctors.Remove(doctor);
            ClinicAppDbContext.SaveChanges();
        }

        public List<Appointment> GetAllDoctorAppointments(int id)
        {
            return ClinicAppDbContext.Appointments.Where(appointment => appointment.Doctor.Id == id).ToList();
        }

        public List<Appointment> GetAllDoctorAppointmentsByDay(int id, DateTime Date)
        {
            return ClinicAppDbContext.Appointments.AsEnumerable()
                .Where(appointment => appointment?.Doctor?.Id == id &&
                        appointment.StartDate.ToShortDateString() == Date.ToShortDateString())
                .ToList();
        }

        public bool IsAvailableforAnAppointmentByDate(int id, DateTime Date)
        {
            List<Appointment> appointments = GetAllDoctorAppointmentsByDay(id, Date);

            if (appointments.Count >= 12)
                return false;

            double totalHours = 0;
            foreach (Appointment appointment in appointments)
            {
                totalHours += NumberOfHoursBetweenDates(appointment.StartDate, appointment.EndDate);
            }

            if(totalHours >= 8)
                return false;

            return true;
        }
        double NumberOfHoursBetweenDates(DateTime FirstDate, DateTime EndDate)
        {
           return (EndDate - FirstDate).TotalHours;
        }

        public List<Doctor> GetDoctorsWithMostAppointmentsByDate(DateTime Date)
        {
            throw new NotImplementedException();
        }

        public List<Doctor> GetDoctorsWithAppointmentsExceedingSixHoursByDate(DateTime Date)
        {
            List<Doctor> doctorsWhoExceedSixHours = new List<Doctor>();
            List<Doctor> doctorsWhoWorkedThatDate = new List<Doctor>();

            List<Appointment> appointmentsThatDate = ClinicAppDbContext.Appointments.
                AsEnumerable().Where(app=> app.StartDate.ToShortDateString == Date.ToShortDateString).ToList();
            
            foreach (Appointment appointment in appointmentsThatDate)
            {
                Doctor doctor = new Doctor();
                doctor = appointment?.Doctor;
                doctorsWhoWorkedThatDate.Add(doctor);

            }

            foreach (var doctor in doctorsWhoWorkedThatDate)
            {
                if(CalculateTotalDoctorHoursInADay(doctor.Id, Date) >= 6)
                {
                    doctorsWhoExceedSixHours.Add(doctor);
                }

            }

            return doctorsWhoExceedSixHours;
        }
        public Double CalculateTotalDoctorHoursInADay(int doctorId, DateTime Date)
        {
            List<Appointment> appointments = GetAllDoctorAppointmentsByDay(doctorId, Date);
            double totalWorkedHours = 0;
            
            foreach (var appointment in appointments)
            {
                totalWorkedHours += (appointment.EndDate - appointment.StartDate).TotalHours;
            }

            return totalWorkedHours;
        }
    }
}
