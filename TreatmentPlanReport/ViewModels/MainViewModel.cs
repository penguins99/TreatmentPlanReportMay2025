using DoseMetricExample.Helpers;
using DVHPlot.ViewModels;
using Microsoft.Win32;
using OxyPlot.Wpf;
using PDFtoAria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using TreatmentPlanReport.Views;
using VMS.SF.Infrastructure.Contracts.Logging.HipaaEvents;
using VMS.TPS.Common.Model.API;

namespace TreatmentPlanReport.ViewModels
{
    public class MainViewModel
    {
        private Patient _patient;
        private User _user;

        public RelayCommand PrintCommand { get; private set; }
        public RelayCommand ARIAPostCommand { get; private set; }

        public MainViewModel(Patient patient, PlanSetup plan, User user)
        {

            //make ESAPI objects available to other methods.
            _patient = patient;
            _user = user;
            PatientViewModel = new PatientViewModel(patient);
            PlanViewModel = new PlanViewModel(plan);
            FieldViewModel = new FieldViewModel(plan);
            EventHelper eventHelper = new EventHelper();
            DVHViewModel = new DVHViewModel(plan, eventHelper);
            DVHSelectionViewModel = new DVHSelectionViewModel(plan, eventHelper);
            PrintCommand = new RelayCommand(OnPrint);
            ARIAPostCommand = new RelayCommand(OnARIAPost);
        }

        private void OnARIAPost(object obj)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "PDF (*.pdf)|*.pdf";
            if (file.ShowDialog() == true)
            {
                var BinaryContent = File.ReadAllBytes(file.FileName);
                CustomInsertDocumentsParameter.PostDocumentData(_patient.Id, _user,
                    BinaryContent, "Plan Report",
                    new VMS.OIS.ARIALocal.WebServices.Document.Contracts.DocumentType { DocumentTypeDescription = "Plan Report" });
            }
        }

        private void OnPrint(object obj)
        {
            FlowDocument fd = new FlowDocument { FontSize = 12, FontFamily = new System.Windows.Media.FontFamily("Arial") };
            fd.Blocks.Add(new Paragraph(new Run("Treatment Plan Report")));
            fd.Blocks.Add(new BlockUIContainer(new PatientView { DataContext = PatientViewModel }));
            fd.Blocks.Add(new BlockUIContainer(new PlanView { DataContext = PlanViewModel }));


            foreach (var field in FieldViewModel.Fields)
            {
                FieldViewModel.SelectedField = field;
                FieldView fieldView = new FieldView();
                var grid = fieldView.FindName("FieldDetailGrid") as Grid;
                if (grid != null)
                {
                    grid.DataContext = field;
                }
                fd.Blocks.Add(new BlockUIContainer(fieldView));

            }

                    
            BitmapSource bmp = new PngExporter().ExportToBitmap(DVHViewModel.DVHPlotModel);
            fd.Blocks.Add(new BlockUIContainer(new System.Windows.Controls.Image
            {
                Source = bmp,
                Height = 600,
                Width = 725
            }));
            System.Windows.Controls.PrintDialog printer = new System.Windows.Controls.PrintDialog();
            //printer.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
            fd.PageHeight = 1056;
            fd.PageWidth = 816;
            fd.PagePadding = new System.Windows.Thickness(50);
            fd.ColumnGap = 0;
            fd.ColumnWidth = 816;
            IDocumentPaginatorSource source = fd;
            if (printer.ShowDialog() == true)
            {
                printer.PrintDocument(source.DocumentPaginator, "TreatmentPlanReport");
            }
        }

        //The spelling of these properties must match the biding expressions in the MainView.xaml file.
        public PatientViewModel PatientViewModel { get; }
        public PlanViewModel PlanViewModel { get; }
        public FieldViewModel FieldViewModel { get; }
        public DVHViewModel DVHViewModel { get; }
        public DVHSelectionViewModel DVHSelectionViewModel { get; }
    }
}
