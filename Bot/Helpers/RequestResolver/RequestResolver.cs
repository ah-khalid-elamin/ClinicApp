using Bot.Helpers.DataSync;
using Common.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Helpers.RequestResolver
{
    public class RequestResolver : DataSyncHelper, IRequestResolver
    {
        private readonly DoctorService DoctorService;
        private readonly PatientService PatientService;
        private readonly AppointmentService AppointmentService;

        public RequestResolver(DoctorService doctorService, PatientService patientService, AppointmentService appointmentService)
        {
            this.DoctorService = doctorService;
            this.PatientService = patientService;
            this.AppointmentService = appointmentService;
        }
        public async Task<object> Resolve(string message)
        {
            object res = "";

            if (message.ToLower().Contains("list") && message.ToLower().Contains("doctors"))
            {
                List<Doctor> doctors = await queryDoctors();
                foreach (Doctor doctor in doctors)
                {
                    res += $"ID: {doctor.Id} Name: {doctor.Name} Speciality: {doctor.Speciality}\n";
                }
                
            }
            else if (message.ToLower().Contains("list") && message.ToLower().Contains("patients"))
            {
                List<Patient> patients = await queryPatients();
                foreach (Patient patient in patients)
                {
                    res += $"{patient.Id} {patient.Name}\n";
                }
            }
            else if (message.ToLower().Contains("list") && message.ToLower().Contains("appointments"))
            {
                List<Appointment> appointments = await queryAppointments();
                foreach (Appointment appointment in appointments)
                {
                    //Doctor doctor = DoctorService.GetDoctor(appointment?.Doctor.Id);
                    //Patient patient = PatientService.GetPatient(appointment.Patient.Id);
                    res += $"{appointment.Id} Start: {appointment.StartDate} End: {appointment.EndDate} Doctor: {appointment?.Doctor?.Name} Patient: {appointment?.Patient?.Name}\n";
                }
            }
            else if (message.ToLower().Contains("book"))
            {
                List<Appointment> appointments = AppointmentService.GetAllAppointments();
                foreach (Appointment appointment in appointments)
                {
                    //Doctor doctor = DoctorService.GetDoctor(appointment?.Doctor.Id);
                    //Patient patient = PatientService.GetPatient(appointment.Patient.Id);
                    res += $"{appointment.Id} Start: {appointment.StartDate} End: {appointment.EndDate} Doctor: {appointment?.Doctor?.Name} Patient: {appointment?.Patient?.Name}\n";
                }
            }
            else
            {
                res = "Unsupported operation";
            }

            return res;
        }

    }
}
