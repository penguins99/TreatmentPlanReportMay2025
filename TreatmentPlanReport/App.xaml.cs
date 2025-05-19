using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TreatmentPlanReport.ViewModels;
using TreatmentPlanReport.Views;


//Reference ESAPI libraries
//Reference with Alias beacuase the Application class is also in System.Windows
using esapi = VMS.TPS.Common.Model.API;

namespace TreatmentPlanReport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            using (esapi.Application app = esapi.Application.CreateApplication())
            {
                //TODO: Get Patient Info from Context.
                esapi.Patient patient = app.OpenPatientById("RapidPlan-02");//hard code patient for now.

                esapi.Course course = patient.Courses.First(c => c.Id == "Demo");
                esapi.PlanSetup plan = course.PlanSetups.First(p => p.Id == "Boost Calc");

                //Instead of creating a new Window, use MainView
                MainView mainView = new MainView();
                MainViewModel mainViewModel = new MainViewModel(patient, plan);
                mainView.DataContext = mainViewModel;
                mainView.ShowDialog();

                //Window window = new Window();
                //PatientView patientView = new PatientView();//using TreatmentPlanReport.Views;
                //patientView.DataContext = new PatientViewModel(patient);//using TreatmentPlanReport.ViewModels;
                //window.Content = patientView;// app.CurrentUser.Name;
                //window.ShowDialog();//pause the app here.
                //window.Show();//continue the app (then the app object is disposed).
            }
        }
    }
}
