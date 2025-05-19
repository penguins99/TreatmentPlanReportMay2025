using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreatmentPlanReport.Models;
using VMS.TPS.Common.Model.API;

namespace TreatmentPlanReport.ViewModels
{
    public class FieldViewModel
    {
        public FieldModel SelectedField { get; set; }
        public List<FieldModel> Fields { get; set; }
        public FieldViewModel(PlanSetup plan)
        {
            Fields = new List<FieldModel>();
            foreach (Beam beam in plan.Beams.Where(b => !b.IsSetupField).OrderBy(b => b.BeamNumber))
            {
                Fields.Add(new FieldModel(plan, beam));
            }
        }
    }
}
