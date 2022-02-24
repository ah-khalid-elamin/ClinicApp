using ClinicApp.Models;
using ClinicApp.Services;
using ClinicApp.Wrappers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService appointmentService;
        public AppointmentsController(AppointmentService _appointmentService) 
        {
            this.appointmentService = _appointmentService;
        }
        // GET: api/<AppointmentController>
        [HttpGet]
        [EnableQuery]
        public IQueryable<Appointment> GetAllAppointments()
        {
            return appointmentService.GetAllAppointments()
                .AsQueryable();
        }

        [HttpGet("CsvExport")]
        public FileResult ExportDoctorsToCsv()
        {
            var appointments = appointmentService.ExportAppointmentsToCsv();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var app in appointments)
            {
                sb.Append(app);
                sb.Append("\r\n");

            }
            return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "appointments.csv");
        }

        // GET api/<AppointmentController>/5
        [HttpGet("{id}")]
        public Response<Appointment> GetAppointmentDetails(int id)
        {
            return new Response<Appointment>(
                StatusCodes.Status200OK,
                "Retrieved Successfully",
                appointmentService.GetAppointmentDetails(id));
        }

        // POST api/<AppointmentController>
        [Authorize( Roles  = "Patient")]
        [HttpPost]
        [Route("book-appointment")]
        public Response<Appointment> Post([FromBody] Appointment appointment)
        {
            try
            {
                return new Response<Appointment>(
                 StatusCodes.Status200OK,
                 "Appointment Booked Successfully",
                  appointmentService.BookAnAppointment(appointment));
                

            }
            catch (Exception e)
            {
                return new Response<Appointment>(
                StatusCodes.Status500InternalServerError,
                e.Message,
                 appointment);
            }        
        }

        // PUT api/<AppointmentController>/5
        [HttpPut("{id}")]
        public Response<Appointment> Put(int id, [FromBody] Appointment appointment)
        {
            return new Response<Appointment>(
                StatusCodes.Status200OK,
                "Updated Successfully",
                appointmentService.UpdateAnAppointment(id, appointment));
        }

        // DELETE api/<AppointmentController>/5
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("cancel-appointment/{id}")]
        public Response<String> CancelAnAppointment(int id)
        {
            appointmentService.CancelAppointment(id);

            return new Response<String>(
                StatusCodes.Status200OK,
                "Appointment Canceled Successfully.",
                String.Empty);
        }
    }
}
