using Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.Helpers.DataSync
{
    public class DataSyncHelper
    {
        public async Task<HttpClient> GetHttpClientAsync(LoginModel loginModel)
        {
            HttpClient client = new HttpClient();

            string token = await GetTokenAsync(loginModel);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return client;
        }
        public async Task<HttpClient> GetPatientClientAsync()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Username = "patient0001";
            loginModel.Password = "Patient0001@";

            return await GetHttpClientAsync(loginModel);
        }
        public async Task<HttpClient> GetDoctorClientAsync()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Username = "doctor0001";
            loginModel.Password = "Doctor0001@";

            return await GetHttpClientAsync(loginModel);

        }
        public async Task<HttpClient> GetAdminClientAsync()
        {
            LoginModel loginModel = new LoginModel();
            loginModel.Username = "Admin0001";
            loginModel.Password = "Admin0001@";

            return await GetHttpClientAsync(loginModel);

        }
        private async Task<string> GetTokenAsync(LoginModel loginModel)
        {
            var client = new HttpClient();
            var res = await client.PostAsync(URIHelpers.Login_Controller, 
                new StringContent(JsonConvert.SerializeObject(loginModel), 
                Encoding.UTF8, "application/json"));

            return await res.Content.ReadAsStringAsync();
        }
        public async Task<List<Doctor>> queryDoctors()
        {
            HttpClient client = await GetDoctorClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Doctors_Controller}");
            var list = JsonConvert.DeserializeObject<List<Doctor>>(res);
            return list;
        }
        public async Task<List<Patient>> queryPatients()
        {
            HttpClient client = await GetDoctorClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Patients_Controller}");
            var list = JsonConvert.DeserializeObject<List<Patient>>(res);
            return list;
        }

        public async Task<List<Appointment>> queryAppointments()
        {
            HttpClient client = await GetAdminClientAsync();
            var res = await client.GetStringAsync($"{URIHelpers.Appointments_Controller}");
            var list = JsonConvert.DeserializeObject<List<Appointment>>(res);
            return list;
        }

    }
}
