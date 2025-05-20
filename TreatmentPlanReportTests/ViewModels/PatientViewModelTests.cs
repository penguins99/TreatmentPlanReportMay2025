using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreatmentPlanReport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreatmentPlanReport.ViewModels.Tests
{
    [TestClass()]
    public class PatientViewModelTests
    {
        [TestMethod()]
        public void GetDateOfBirthTestNull()
        {
            //arrange
            string expected = "No DOB";
            //act
            PatientViewModel patientViewModel = new PatientViewModel();
            string actual = patientViewModel.GetDateOfBirth(null);
            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetDateOfBirthTestDate()
        {
            //arrange
            string expected = "10/31/2025";
            //act
            PatientViewModel patientViewModel = new PatientViewModel();
            string actual = patientViewModel.GetDateOfBirth(new DateTime(2025, 10, 31));
            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}