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

            context.Setup(context => context.Appointments).Returns(setMock.Object);
            appointmentService = new AppointmentServiceImpl(context.Object, doctorService.Object);

        }
        [Test]
        public void InvalidAppointmentDurationLessThan15MinutesTest()
        {
            //arrange 
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Parse("2022-02-01 09:00"),
                EndDate = DateTime.Parse("2022-02-01 09:10"),
                Doctor = new Doctor(),
                Patient = new Patient(),
            };


            // action and verify
            var exception = Assert.Throws<InvalidOperationException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("There is an error with the appointment duration", exception.Message);
        }
        [Test]
        public void InvalidAppointmentDurationMoreThanTwoHoursTest()
        {
            //arrange 
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Parse("2022-02-01 09:00"),
                EndDate = DateTime.Parse("2022-02-01 12:00"),
                Doctor = new Doctor(),
                Patient = new Patient(),
            };


            // action and verify
            var exception = Assert.Throws<InvalidOperationException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("There is an error with the appointment duration", exception.Message);
        }
        [Test]
        public void InvalidAppointmentBeforeWorkingHoursTest()
        {
            //arrange 
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Parse("2022-02-01 05:00"),
                EndDate = DateTime.Parse("2022-02-01 07:00"),
                Doctor = new Doctor(),
                Patient = new Patient(),
            };


            // action and verify
            var exception = Assert.Throws<InvalidOperationException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("Clinic opens at 9:00 AM", exception.Message);
        }
        [Test]
        public void InvalidAppointmentAfterWorkingHoursTest()
        {
            //arrange 
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Parse("2022-02-01 22:00"),
                EndDate = DateTime.Parse("2022-02-01 23:00"),
                Doctor = new Doctor(),
                Patient = new Patient(),
            };


            // action and verify
            var exception = Assert.Throws<InvalidOperationException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("Clinic closes at 9:00 PM", exception.Message);
        }
        [Test]
        public void InvalidAppointmentNoDoctor()
        {
            //arrange 
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Parse("2022-02-01 09:00"),
                EndDate = DateTime.Parse("2022-02-01 10:00"),
                Patient = new Patient(),
            };


            // action and verify
            var exception = Assert.Throws<ArgumentNullException>(() => {
                appointmentService.BookAnAppointment(appointment);
            });

            Assert.AreEqual("Doctor is missing", exception.ParamName);

        }
        [Test]
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
        [Test]
        public void FixTheBuildTest()
        {
            Assert.Pass();
        }


    }
}
