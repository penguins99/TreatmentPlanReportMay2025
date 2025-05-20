using DoseMetricExample.Helpers;
using DVHPlot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace TreatmentPlanReport.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(Patient patient, PlanSetup plan)
        {
            PatientViewModel = new PatientViewModel(patient);
            PlanViewModel = new PlanViewModel(plan);
            FieldViewModel = new FieldViewModel(plan);
            EventHelper eventHelper = new EventHelper();
            DVHViewModel = new DVHViewModel(plan, eventHelper);
            DVHSelectionViewModel = new DVHSelectionViewModel(plan, eventHelper);
        }

        //The spelling of these properties must match the biding expressions in the MainView.xaml file.
        public PatientViewModel PatientViewModel { get; }
        public PlanViewModel PlanViewModel { get; }
        public FieldViewModel FieldViewModel { get; }
        public DVHViewModel DVHViewModel { get; }
        public DVHSelectionViewModel DVHSelectionViewModel { get; }
    }
}
