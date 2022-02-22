using ClinicApp.Models;
using ClinicApp.Services;
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
        public List<Appointment> GetAllAppointments()
        {
            return appointmentService.GetAllAppointments();
        }

        // GET api/<AppointmentController>/5
        [HttpGet("{id}")]
        public Appointment GetAppointmentDetails(int id)
        {
            return appointmentService.GetAppointmentDetails(id);
        }

        // POST api/<AppointmentController>
        [Authorize( Roles  = "Patient")]
        [HttpPost]
        [Route("book-appointment")]
        public IActionResult Post([FromBody] Appointment appointment)
        {
            try
            {
                appointmentService.BookAnAppointment(appointment);
                return Ok(appointment);

            }
            catch (Exception e)
            {

                 return BadRequest(e.Message);
            }        
        }

        // PUT api/<AppointmentController>/5
        [HttpPut("{id}")]
        public Appointment Put(int id, [FromBody] Appointment appointment)
        {
            return appointmentService.UpdateAnAppointment(id, appointment);
        }

        // DELETE api/<AppointmentController>/5
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("cancel-appointment/{id}")]
        public void CancelAnAppointment(int id)
        {
            appointmentService.CancelAppointment(id);
        }
    }
}
