using Common.Models;
using Common.Services;
using Common.Wrappers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles ="Patient")]
        [HttpGet("{id}")]
        public Patient Get(string id)
        {
            return PatientService.GetPatient(id);
        }

        [HttpGet("{id}/previous-appointments")]
        public IQueryable<Appointment> GetPreviousAppointments(string id)
        {
            return PatientService.GetPatientPreviousAppointments(id)
                .AsQueryable();
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Patient patient)
        {
            PatientService.Update(id,patient);
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            PatientService?.Delete(id);
        }
    }
}
