using Common.Models;
using Common.Services;
using Common.Contexts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppTests
{
    public class AppointmentTests
    {
        public AppointmentService appointmentService { get; set; }
        public Mock<DbSet<Appointment>> setMock = new Mock<DbSet<Appointment>>();
        public Mock<ClinicAppDbContext> context = new Mock<ClinicAppDbContext>();
        public Mock<DoctorService> doctorService { get; set; } = new Mock<DoctorService>();

        [SetUp]
        public void SetUp()
        {


        }
        [Test]
        public void InvalidAppointmentDurationTest()
        {
            var mockContext = new Mock<ClinicAppDbContext>();
            var doctorService = new Mock<DoctorService>();

            // action and verify
            var exception = Assert.Throws<ArgumentNullException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("Doctor is missing", exception.ParamName);

        }
        public void InvalidAppointmentNoPatient()
        {
            //arrange 
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Parse("2022-02-01 09:00"),
                EndDate = DateTime.Parse("2022-02-01 10:00"),
                Doctor = new Doctor()
            };


            // action and verify
            var exception = Assert.Throws<ArgumentNullException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("Patient is missing", exception.ParamName);

        }


    }
}
