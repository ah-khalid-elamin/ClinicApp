using ClinicApp.Models;
using ClinicApp.Services;
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
        [HttpGet]
        public List<Doctor> GetAllDoctors()
        {
            return DoctorService.GetDoctors();
        }

        // GET api/doctors/5
        [HttpGet("{id}")]
        public Doctor GetDoctorInfo(int id)
        {
            return DoctorService.GetDoctor(id);
        }
        [HttpGet("{id}/appointments")]
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
