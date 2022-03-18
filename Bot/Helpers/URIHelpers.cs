namespace Bot.Helpers
{
    public class URIHelpers
    {
        public static string Token { get; set; }
        public static string Base_URI = "https://openclinic.azurewebsites.net";
        public static string Doctors_Controller = $"{Base_URI}/api/doctors";
        public static string Patients_Controller = $"{Base_URI}/api/patients";
        public static string Appointments_Controller = $"{Base_URI}/api/appointments";
        public static string Login_Controller = $"{Base_URI}/api/Account/login";
        public static string Register_Patient_Controller = $"{Base_URI}/api/Account/resgister-patient";

        public static void SetBaseURL(string url)
        {
            Base_URI = url;
        }
    }
}
