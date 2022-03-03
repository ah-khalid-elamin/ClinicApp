using Common.Contexts;
using Common.Models;
using Common.Services;
using Common.Services.Impl;
using Microsoft.EntityFrameworkCore;
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
        public PatientService patientService { get; set; }
        public Mock<DbSet<Patient>> setMock = new Mock<DbSet<Patient>>();
        public Mock<ClinicAppDbContext> context = new Mock<ClinicAppDbContext>();
        public Patient patient { get; set; }

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void SavePatientTest()
        {

            //arrange

            context.Setup(context => context.Patients).Returns(GetDbSetMock<Patient>().Object);
            patientService = new PatientServiceImpl(context.Object);


            Patient patient = new Patient()
            {
                Name = "Patient0",
                BirthDate = DateTime.Now,
                Gender = "Undefined"
            };

            //action
            patientService.Save(patient);

            //verify
            context.Verify(c => c.Patients.Add(patient), Times.Once);
            context.Verify(s => s.SaveChanges(), Times.Once);
        }
        [Test]
        public void UpdateAnExistingPatientTest()
        {
            //arrange
            Patient patient = new Patient()
            {
                Id = 1,
                Name = "Patient0",
                BirthDate = DateTime.Now,
                Gender = "Undefined"
            };

            List<Patient> list = new List<Patient>()
            {
                patient
            };

            context.Setup(context => context.Patients).Returns(GetDbSetMock<Patient>(
                list

                ).Object);


            patientService = new PatientServiceImpl(context.Object);


            //action
            patientService.Update(patient.Id, patient);

            //verify
            context.Verify(c => c.Patients.Update(patient), Times.Once);
            context.Verify(s => s.SaveChanges(), Times.AtLeastOnce);
        }
        [Test]
        public void UpdateNonExistingPatientTest()
        {
            //arrange

            context.Setup(context => context.Patients).Returns(GetDbSetMock<Patient>().Object);
            patientService = new PatientServiceImpl(context.Object);

            Patient patient = new Patient()
            {
                Id = 1,
                Name = "Patient0",
                BirthDate = DateTime.Now,
                Gender = "Undefined"
            };

            //verify
            Assert.Throws<Exception>(() => {
                patientService.Update(patient.Id, patient); //Action
            });

        }
        private static Mock<DbSet<T>> GetDbSetMock<T>(IEnumerable<T> items = null) where T : class
        {
            if (items == null)
            {
                items = new T[0];
            }

            var dbSetMock = new Mock<DbSet<T>>();
            var q = dbSetMock.As<IQueryable<T>>();

            q.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator);

            return dbSetMock;
        }
    }
}