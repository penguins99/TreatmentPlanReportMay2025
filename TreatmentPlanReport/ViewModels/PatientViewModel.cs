using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace TreatmentPlanReport.ViewModels
{
    //make classoublic
    public class PatientViewModel
    {
        public string PatientId { get; set; }

        public string Name { get; set; }

        public string Hospital { get; set; }

        public string DateOfBirth { get; set; }

        //Constructor can pass in arguments as well as set properties.

        public PatientViewModel(Patient patient)
        {
            PatientId = patient.Id;
            Name = $"{patient.LastName}, {patient.FirstName}";//string Interpolation!!
            Hospital = patient.Hospital.Id;
            DateOfBirth = patient.DateOfBirth == null ?//ternary operator
                "No DOB" ://true part.
                patient.DateOfBirth.Value.ToShortDateString();//false part.
        }
    }
}
