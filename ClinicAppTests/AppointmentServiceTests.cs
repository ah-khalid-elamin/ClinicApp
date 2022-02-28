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
        [SetUp]
        public void SetUp()
        {


        }
        [Test]
        public void InvalidAppointmentDurationTest()
        {
            var mockContext = new Mock<ClinicAppDbContext>();
            var doctorService = new Mock<DoctorService>();

            var mockSet = new Mock<DbSet<Appointment>>();

            mockContext.Setup(m => m.Appointments).Returns(mockSet.Object);
            var appointmentService = new Mock<AppointmentService>();

            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Doctor = null,
                Patient = null,
            };

            appointmentService.BookAnAppointment(appointment);
        }

    }
}
