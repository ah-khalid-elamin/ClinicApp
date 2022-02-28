using Common.Models;
using Common.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppTests
{
    public class PatientTests
    {
        Mock<PatientService> patientService;
        Patient patient;


        [SetUp]
        public void SetUp()
        {
            patient = new Patient()
            {
                Id = 1,
                Name = "User name",
                BirthDate = DateTime.Now,
                Gender = "Undefined"
            };

            patientService = new Mock<PatientService>();
            patientService.Setup(p => p.Save(patient)).Returns(patient);

        }
        [Test]
        public void SavePatientTest()
        { 
           Patient p = patientService.Object.Save(patient);
           patientService.Verify(p => p.Save(patient), Times.Once);

        }
        [Test]
        public void UpdatePatientTest()
        {
            Patient p = patientService.Object.Update(1, patient);
            patientService.Verify(p => p.Update(patient.Id, patient), Times.Once);

        }

    }
}
