using ClinicApp.Models;
using ClinicApp.Services;
using ClinicApp.Wrappers;
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
        public IQueryable<Doctor> GetDoctors()
        {
             return   DoctorService.GetDoctors().AsQueryable();
        }

        // GET api/doctors/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public Response<Doctor> GetDoctor(int id)
        {
            return new Response<Doctor>
               (
                 StatusCodes.Status200OK
               , "Retrieved Successfully"
               , DoctorService.GetDoctor(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/doctors-exceed-six-hours-list")]
        public Pageable<List<Doctor>> GetDoctorWhoExceedSixHours(DateTime date, [FromQuery] Pagination pagination)
        {
            return new Pageable<List<Doctor>>
               (
                 pagination.Page,
                 pagination.PageSize,
                 StatusCodes.Status200OK
               , "Retrieved Successfully"
               , DoctorService.GetDoctorsWithAppointmentsExceedingSixHoursByDate(date, pagination));
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("{id}/appointments-list")]
        public Pageable<List<Appointment>> GetDoctorAppointments(int id, [FromQuery] Pagination pagination)
        {
            return new Pageable<List<Appointment>>
               (
                 pagination.Page,
                 pagination.PageSize,
                 StatusCodes.Status200OK
               , "Retrieved Successfully"
               , DoctorService.GetAllDoctorAppointments(id, pagination));
        }

        // POST api/<DoctorController>
        [HttpPost]
        public Response<Doctor> Post([FromBody] Doctor doctor)
        {
            return new Response<Doctor>
              (
                StatusCodes.Status201Created
              , "Created Successfully"
              , DoctorService.Save(doctor));

           
        }

        // PUT api/<DoctorController>/5
        [HttpPut("{id}")]
        public Response<Doctor> Put(int id, [FromBody] Doctor doctor)
        {
            return new Response<Doctor>
              (
                StatusCodes.Status200OK
              , "Updated Successfully"
              , DoctorService.Update(id, doctor));
        }

        // DELETE api/<DoctorController>/5
        [HttpDelete("{id}")]
        public Response<String> Delete(int id)
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
