using DoseMetricExample.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVHPlot.Models
{
    public class StructureSelectionModel:ViewModelBase
    {
        public string Id { get; set; }
        private bool bisChecked;
        private EventHelper _eventHelper;

        //private IEventAggregator _eventAggregator;

        public bool bIsChecked
        {
            get { return bisChecked; }
            set 
            { 
                SetProperty(ref bisChecked, value);
                //_eventAggregator.GetEvent<StructureSelectionEvent>().Publish(this);
                _eventHelper.Publish<StructureSelectionModel>("StructureSelectionEvent", this);
            }
        }
        public StructureSelectionModel(EventHelper eventHelper)//IEventAggregator eventAggregator)
        {
            //_eventAggregator = eventAggregator;
            _eventHelper = eventHelper;
        }

    }
}
