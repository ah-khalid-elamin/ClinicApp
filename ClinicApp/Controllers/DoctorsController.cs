using ClinicApp.Models;
using ClinicApp.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService DoctorService;
        public DoctorsController(DoctorService _DoctorService)
        {
            this.DoctorService = _DoctorService;
        }
        // GET: api/<DoctorController>
        [HttpGet]
        public List<Doctor> Get()
        {
            return DoctorService.GetDoctors();
        }

        // GET api/<DoctorController>/5
        [HttpGet("{id}")]
        public Doctor Get(int id)
        {
            return DoctorService.GetDoctor(id);
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
