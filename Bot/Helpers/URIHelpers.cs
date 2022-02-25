namespace Bot.Helpers
{
    public static class URIHelpers
    {
        public const string Base_URI = "https://localhost:7147";
        public const string Doctors_Controller = $"{Base_URI}/api/doctors";
        public const string Patients_Controller = $"{Base_URI}/api/patients";
        public const string Appointments_Controller = $"{Base_URI}/api/appointments";
        public const string Login_Controller = $"{Base_URI}/api/Account/login";
        public const string Register_Patient_Controller = $"{Base_URI}/api/Account/resgister-patient";


    }
}
