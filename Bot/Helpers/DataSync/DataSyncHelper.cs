using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.Helpers.DataSync
{
    public class DataSyncHelper
    {
        private readonly ILogger<DataSyncHelper> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;


        public DataSyncHelper(ILogger<DataSyncHelper> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }
        public async Task<HttpClient> GetHttpClientAsync()
        {
            HttpClient client = new HttpClient();
            string token = await GetTokenAsync();
            return client;
        }
        private async Task<string> GetTokenAsync()
        {
            string[] scopesToAccessDownstreamApi = new string[] { "api://fd3d3dd9-80dd-4a82-8c3b-3c4dfca593b0/ReadAccess" };
            var token = await _tokenAcquisition.GetAccessTokenForUserAsync(scopesToAccessDownstreamApi);
            return token;
        }
        public async Task<List<Doctor>> queryDoctors()
        {
            HttpClient client = await GetHttpClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Doctors_Controller}");
            var list = JsonConvert.DeserializeObject<List<Doctor>>(res);
            return list;
        }
        public async Task<List<Doctor>> queryDoctorsById(int id)
        {
            HttpClient client = await GetHttpClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Doctors_Controller}?$filter=id eq {id}");
            var doc = JsonConvert.DeserializeObject<List<Doctor>>(res);
            return doc;
        }
        public async Task<List<Appointment>> queryDoctorAppointmentsById(string id)
        {
            HttpClient client = await GetHttpClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Appointments_Controller}?$filter=Doctor/id eq {id}");
            var app = JsonConvert.DeserializeObject<List<Appointment>>(res);
            return app;
        }

        public async Task<List<Doctor>> queryDoctorsByName(string name)
        {
            HttpClient client = await GetHttpClientAsync();
            var req = URIHelpers.Doctors_Controller + "?$filter=contains(tolower(name),tolower('" + name.Trim() + "'))";
            var res = await client.GetStringAsync(req);
            List<Doctor> docs = JsonConvert.DeserializeObject<List<Doctor>>(res);
            return docs;
        }

        public async Task<List<Patient>> queryPatientsByName(string name)
        {
            HttpClient client = await GetHttpClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Patients_Controller}?$filter=contains(tolower(name),tolower('{ name.Trim()}'))");
            var list = JsonConvert.DeserializeObject<List<Patient>>(res);
            return list;
        }

        public async Task<List<Patient>> queryPatients()
        {
            HttpClient client = await GetHttpClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Patients_Controller}");
            var list = JsonConvert.DeserializeObject<List<Patient>>(res);
            return list;
        }

        public async Task<List<Appointment>> queryAppointments()
        {
            HttpClient client = await GetHttpClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Appointments_Controller}");
            var list = JsonConvert.DeserializeObject<List<Appointment>>(res);
            return list;
        }
        public async Task<Appointment> queryAppointment()
        {
            HttpClient client = await GetHttpClientAsync();
            var req = await client.GetStringAsync($"{URIHelpers.Appointments_Controller}");
            var res = JsonConvert.DeserializeObject<Appointment>(req);
            return res;
        }

    }
}
