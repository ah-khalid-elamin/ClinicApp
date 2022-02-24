using ClinicApp.Models;
using ClinicApp.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        public PatientService PatientService { get; set; }

        public PatientsController(PatientService _patientService)
        {
            PatientService = _patientService;
        }
        // GET: api/<PatientController>
        [HttpGet]
        [EnableQuery]
        public IQueryable<Patient> Get()
        {
            return PatientService.GetPatients()
                   .AsQueryable();
        }

        [HttpGet("CsvExport")]
        public FileResult ExportDoctorsToCsv()
        {
            var patients = PatientService.ExportPatientsToCsv();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach(var patient in patients)
            {
                sb.Append(patient);
                sb.Append("\r\n");

            }
            return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "patients.csv");
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public Patient Get(int id)
        {
            return PatientService.GetPatient(id);
        }

        [HttpGet("{id}/previous-appointments")]
        public IQueryable<Appointment> GetPreviousAppointments(int id)
        {
            return PatientService.GetPatientPreviousAppointments(id)
                .AsQueryable();
        }

        // POST api/<PatientController>
        [HttpPost]
        public void Post([FromBody] Patient patient)
        {
            PatientService.Save(patient);
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Patient patient)
        {
            PatientService.Update(id,patient);
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            PatientService?.Delete(id);
        }
    }
}
