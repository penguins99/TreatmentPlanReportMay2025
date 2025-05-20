using DoseMetricExample.Helpers;
using DVHPlot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace DVHPlot.ViewModels
{
    public class DVHSelectionViewModel
    {
        private PlanSetup _plan;
        private EventHelper _eventHelper;
        //private IEventAggregator _eventAggregator;
        public ObservableCollection<StructureSelectionModel> SelectionStructures {get; private set;}
        public DVHSelectionViewModel(PlanSetup plan,
            //IEventAggregator eventAggregator
            EventHelper eventHelper)
        {
            _plan = plan;
            //_eventAggregator = eventAggregator;
            _eventHelper = eventHelper;
            SelectionStructures = new ObservableCollection<StructureSelectionModel>();
            SetInitialStructures();
        }

        private void SetInitialStructures()
        {
            foreach(Structure s in _plan.StructureSet.Structures.Where(x=>!x.IsEmpty && x.DicomType!="MARKER" && x.DicomType != "SUPPORT"))
            {
                SelectionStructures.Add(new StructureSelectionModel(_eventHelper)
                {
                    Id = s.Id,
                    bIsChecked = _plan.StructuresSelectedForDvh.Contains(s)
                });
            }
        }
    }
}
