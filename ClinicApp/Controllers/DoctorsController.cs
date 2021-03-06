using Common.Models;
using Common.Services;
using Common.Wrappers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService DoctorService;
        public DoctorsController(DoctorService _DoctorService)
        {
            this.DoctorService = _DoctorService;
        }
        // GET: api/doctors
        [Authorize(Roles = "Admin, Doctor, Patient")]
        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Doctor>> GetDoctors()
        {
            try
            {
                return Ok(DoctorService.GetDoctors().AsQueryable());
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpGet("CsvExport")]
        public FileResult ExportDoctorsToCsv()
        {
            var doctors = DoctorService.ExportDoctorsToCsv();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var doc in doctors)
            {
                sb.Append(doc);
                sb.Append("\r\n");

            }
            return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "doctors.csv");
        }

        // GET api/doctors/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public Response<Doctor> GetDoctor(string id)
        {
            return new Response<Doctor>
               (
                 StatusCodes.Status200OK
               , "Retrieved Successfully"
               , DoctorService.GetDoctor(id));
        }

        // GET api/doctors/5
        [HttpGet("{id}/slots")]
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public Response<List<Slot>> GetDoctor(string id, [FromQuery] DateTime date)
        {
            return new Response<List<Slot>>
               (
                 StatusCodes.Status200OK
               , "Retrieved Successfully"
               , DoctorService.GetDoctorAvailableSlots(id,date));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("doctors-exceed-six-hours-list")]
        [EnableQuery]
        public IQueryable<Doctor> GetDoctorWhoExceedSixHours([FromQuery] DateTime date)
        {
            return DoctorService.GetDoctorsWithAppointmentsExceedingSixHoursByDate(date)
                   .AsQueryable();
        }
        [HttpGet("doctors-with-most-appointments")]
        [EnableQuery]
        public IQueryable<Doctor> GetDoctorsWithMostAppointments([FromQuery] DateTime date)
        {
            return DoctorService.GetDoctorsWithMostAppointmentsByDate(date)
                   .AsQueryable();
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("{id}/appointments-list")]
        [EnableQuery]
        public IQueryable<Appointment> GetDoctorAppointments(string id, [FromQuery] Pagination pagination)
        {
            return DoctorService.GetAllDoctorAppointments(id)
                .AsQueryable();
        }

        // PUT api/<DoctorController>/5
        [HttpPut("{id}")]
        public Response<Doctor> Put(string id, [FromBody] Doctor doctor)
        {
            return new Response<Doctor>
              (
                StatusCodes.Status200OK
              , "Updated Successfully"
              , DoctorService.Update(id, doctor));
        }

        // DELETE api/<DoctorController>/5
        [HttpDelete("{id}")]
        public Response<String> Delete(string id)
        {
            DoctorService.Delete(id);

            return new Response<String>
              (
                StatusCodes.Status200OK
              , "Deleted Successfully"
              , string.Empty);
        }
    }
}
