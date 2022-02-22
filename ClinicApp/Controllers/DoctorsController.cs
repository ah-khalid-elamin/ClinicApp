using ClinicApp.Models;
using ClinicApp.Services;
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
        public List<Doctor> GetDoctors()
        {
            return DoctorService.GetDoctors();
        }

        // GET api/doctors/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public Doctor GetDoctor(int id)
        {
            return DoctorService.GetDoctor(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/doctors-exceed-six-hours-list")]
        public List<Doctor> GetDoctorWhoExceedSixHours(DateTime date)
        {
            return DoctorService.GetDoctorsWithAppointmentsExceedingSixHoursByDate(date);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("{id}/appointments-list")]
        public List<Appointment> GetDoctorAppointments(int id)
        {
            return DoctorService.GetAllDoctorAppointments(id);
        }

        // POST api/<DoctorController>
        [HttpPost]
        public void Post([FromBody] Doctor doctor)
        {
            DoctorService.Save(doctor);
        }

        // PUT api/<DoctorController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Doctor doctor)
        {
            DoctorService.Update(doctor);
        }

        // DELETE api/<DoctorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DoctorService.Delete(id);
        }
    }
}
