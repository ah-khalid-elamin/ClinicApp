using Bot.Helpers.AdaptiveCards;
using Bot.Helpers.DataSync;
using Common.Models;
using Common.Services;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bot.Helpers.RequestResolver
{
    public class RequestResolver : DataSyncHelper, IRequestResolver
    {
        private readonly DoctorService DoctorService;
        private readonly PatientService PatientService;
        private readonly AppointmentService AppointmentService;
        private readonly IConfiguration Configuration;

        public RequestResolver(IConfiguration _configuration, ILogger<DataSyncHelper> _DataSyncLogger,
            ITokenAcquisition tokenAcquisition) : base(_DataSyncLogger, tokenAcquisition)
        { 
            this.Configuration = _configuration;
        }

        public async Task<List<Attachment>> Resolve(TeamsChannelAccount user, string message)
        {


            List<Attachment> res = new List<Attachment>();

            if (message.ToLower().Contains("doctor"))
            {
                if (Regex.IsMatch(message, ".*[0-9].*"))
                {
                    //query doctor by id
                    MatchCollection matches = new Regex("[0-9].*", RegexOptions.IgnoreCase).Matches(message);
                    if (matches.Any())
                    {
                        int id = int.Parse(matches.FirstOrDefault().ToString().Trim());
                        List<Doctor> docs = await queryDoctorsById(id);
                        res = AdaptiveCardsHelper.GetDoctorsCards(docs);
                        return res;
                    }
                }
                if (Regex.IsMatch(message, "doctor .*") && !(message.Contains("appointment") || message.Contains("slot")))
                {
                    //query doctor by name
                    MatchCollection matches = new Regex(" .*", RegexOptions.IgnoreCase).Matches(message);
                    if (matches.Any())
                    {
                        string name = matches.FirstOrDefault().ToString();
                        List<Doctor> docs = await queryDoctorsByName(name);
                        res = AdaptiveCardsHelper.GetDoctorsCards(docs);
                        return res;
                    }
                }
                if (Regex.IsMatch(message, "doctor .*slot.*"))
                {
                    //query doctor available slots
                }
                if (Regex.IsMatch(message, "doctor .*appointments"))
                {
                    //query doctor appointments
                    MatchCollection matches = new Regex(" .* ", RegexOptions.IgnoreCase).Matches(message);
                    if (matches.Any())
                    {
                        string name = matches.FirstOrDefault().ToString();
                        List<Doctor> docs = await queryDoctorsByName(name);
                        
                        Doctor doc = docs.FirstOrDefault();
                        List<Appointment> appointments = await queryDoctorAppointmentsById(doc.Id);
                            
                        res = AdaptiveCardsHelper.GetAppointmentsCards(appointments);
                        return res;
                    }

                }
                if (Regex.IsMatch(message, ".*doctors"))
                {
                    List<Doctor> doctors = await queryDoctors();
                    res = AdaptiveCardsHelper.GetDoctorsCards(doctors);
                    return res;
                }
                
            }
            else if (message.ToLower().Contains("patient"))
            {
                if (Regex.IsMatch(message, "patient .*") && !(message.Contains("appointment") || message.Contains("slot")))
                {
                    //query patient by name
                    MatchCollection matches = new Regex(" .*", RegexOptions.IgnoreCase).Matches(message);
                    if (matches.Any())
                    {
                        string name = matches.FirstOrDefault().ToString();
                        List<Patient> patients = await queryPatientsByName(name);
                        res = AdaptiveCardsHelper.GetPatientsCards(patients);
                        return res;
                    }
                }
                if (Regex.IsMatch(message, ".*patients"))
                {
                    List<Patient> patients = await queryPatients();
                    res = AdaptiveCardsHelper.GetPatientsCards(patients);
                    return res;
                }

            }
            else if (message.ToLower().Contains("appointments"))
            {
                List<Appointment> appointments = await queryAppointments();
                res = AdaptiveCardsHelper.GetAppointmentsCards(appointments);
            }
            else if (message.ToLower().Contains("book"))
            {
                Appointment appointment = await queryAppointment();
                res.Add(AdaptiveCardsHelper.GetAppointmentCard(appointment));
                
            }
            else
            {
                res.Add(AdaptiveCardsHelper.GetUnsupportedOperationCard());
            }

            return res;
        }

    }
}
