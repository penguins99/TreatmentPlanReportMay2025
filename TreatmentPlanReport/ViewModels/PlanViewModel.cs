using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace TreatmentPlanReport.ViewModels
{
    public class PlanViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }
        public string ApprovalHistory { get; set; }
        public PlanViewModel(PlanSetup plan)
        {
            Id = plan.Id;
            Name = plan.Name;
            CourseId = plan.Course.Id;
            ApprovalStatus = plan.ApprovalStatus.ToString();
            //Orderby is a linq expression. Here the placeholder represents an approval history.
            Approver = plan.ApprovalHistory.OrderBy(x => x.ApprovalDateTime).Last().UserDisplayName;
            ApprovalHistory = String.Join("\n", plan.ApprovalHistory
            .Select(x => $"{x.ApprovalStatus} -  {x.ApprovalDateTime} by {x.UserId}"));
        }
    }
}
