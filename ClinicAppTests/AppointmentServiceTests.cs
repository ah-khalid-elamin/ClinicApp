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
            Appointment appointment = new Appointment()
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Doctor = null,
                Patient = null,
            };
        }

    }
}
