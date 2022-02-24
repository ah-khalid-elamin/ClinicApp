using ClinicApp.DbContexts;
using ClinicApp.Models;
using ClinicApp.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Services.Impl
{
    public class DoctorServiceImpl : DoctorService
    {
        private readonly ClinicAppDbContext ClinicAppDbContext;

        public DoctorServiceImpl(ClinicAppDbContext _clinicAppDbContext)
        {
            this.ClinicAppDbContext = _clinicAppDbContext;
        }
        public List<Doctor> GetDoctors()
        {
            return ClinicAppDbContext.Doctors
                .ToList();
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
        public Doctor Update(int Id, Doctor doctor)
        {
            Doctor existingDoctor = GetDoctor(Id);
            if (existingDoctor == null)
            {
                throw new Exception("This doctor does not exist");
            }

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
            return ClinicAppDbContext.Appointments.Where(appointment => appointment.Doctor.Id == id)
                .ToList();
        }

        public List<Appointment> GetAllDoctorAppointmentsByDay(int id, DateTime Date)
        {
            return ClinicAppDbContext.Appointments.Include(a => a.Doctor).AsNoTracking().AsEnumerable()
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
            List<Appointment> GroupedAppointmentsThatDayByDoctors = ClinicAppDbContext.Appointments
                .Include(a => a.Doctor).AsNoTracking().AsEnumerable()
                .Where(appointment => appointment.StartDate.ToShortDateString() == Date.ToShortDateString())
                .ToList();

            throw new NotImplementedException();
        }

        public List<Doctor> GetDoctorsWithAppointmentsExceedingSixHoursByDate(DateTime Date)
        {
            List<Doctor> doctorsWhoExceedSixHours = new List<Doctor>();
            List<Doctor> doctorsWhoWorkedThatDate = new List<Doctor>();

            List<Appointment> appointmentsThatDate = ClinicAppDbContext.Appointments.
                AsEnumerable().Where(app=> app.StartDate.ToShortDateString == Date.ToShortDateString)
                .ToList();
            
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

        public List<string> ExportDoctorsToCsv()
        {
            List<string> result = new List<string>();

            List<Doctor> doctors = GetDoctors();

            foreach (var doctor in doctors)
            {
                result.Add(doctor.ToString());
            }

            return result;
        }
    }
}
