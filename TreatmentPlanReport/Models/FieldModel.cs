using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace TreatmentPlanReport.Models
{
    public class FieldModel
    {
        public string FieldId { get; set; }
        public string FieldName { get; set; }
        public string Technique { get; set; }
        public string Energy { get; set; }
        public double FieldX { get; set; }
        public double FieldY { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public string Machine { get; set; }
        public string Isocenter { get; set; }
        public double MU { get; set; }
        public string MLCPlanType { get; set; }
        public int NumControlPoints { get; set; }
        public double SSD { get; set; }
        public double EffectiveDepth { get; set; }
        public double DoseRate { get; set; }
        public string Gantry { get; set; }
        public double Collimator { get; set; }
        public string ToleranceTable { get; set; }
        public double CouchAngle { get; set; }
        public BitmapSource DRR { get; set; }

        public FieldModel(PlanSetup plan, Beam beam)
        {
            bool bIsVarianScale = beam.TreatmentUnit.MachineModel.Contains("Varian");//VarianScale or VarianIEC
            double vScale = 1.0;
            if (bIsVarianScale) { vScale = -1.0; }
            FieldId = beam.Id;
            FieldName = beam.Name;
            Technique = beam.Technique.Id;
            Energy = beam.EnergyModeDisplayName;
            FieldX = (beam.ControlPoints.Max(x => x.JawPositions.X2) - beam.ControlPoints.Min(x => x.JawPositions.X1)) / 10.0;
            FieldY = (beam.ControlPoints.Max(x => x.JawPositions.Y2) - beam.ControlPoints.Min(x => x.JawPositions.Y1)) / 10.0;
            X1 = vScale * beam.ControlPoints.Min(x => x.JawPositions.X1) / 10.0;
            X2 = beam.ControlPoints.Max(x => x.JawPositions.X2) / 10.0;
            Y1 = vScale * beam.ControlPoints.Min(x => x.JawPositions.Y1) / 10.0;
            Y2 = beam.ControlPoints.Max(x => x.JawPositions.Y2) / 10.0;
            Machine = beam.TreatmentUnit.Id;
            Isocenter = GetIsocenter(plan, beam);
            MU = beam.Meterset.Value;
            MLCPlanType = beam.MLCPlanType.ToString();
            //NumControlPoints = beam.ControlPoints.Count();
            SSD = beam.SSD / 10.0;//cm
            DoseRate = beam.DoseRate;
            Gantry = GetGantry(beam);
            Collimator = beam.ControlPoints.First().CollimatorAngle;
            CouchAngle = beam.ControlPoints.First().PatientSupportAngle;
            ToleranceTable = beam.ToleranceTableLabel;
            DRR = BuildDRRImage(beam);
        }

        private BitmapSource BuildDRRImage(Beam beam)
        {
            if (beam.ReferenceImage == null) { return null; }
            var drr = beam.ReferenceImage;
            int[,] pixels = new int[drr.YSize, drr.XSize];
            drr.GetVoxels(0, pixels);//get image pixels out of ESAPI.
            int[] flat_pixels = new int[drr.YSize * drr.XSize];
            //lay out pixels into single array
            for (int i = 0; i < drr.YSize; i++)
            {
                for (int ii = 0; ii < drr.XSize; ii++)
                {
                    flat_pixels[i + drr.XSize * ii] = pixels[i, ii];
                }
            }
            //translate into byte array
            var drr_max = flat_pixels.Max();
            var drr_min = flat_pixels.Min();
            PixelFormat format = PixelFormats.Gray8;//low res image, but only 1 byte per pixel. 
            int stride = (drr.XSize * format.BitsPerPixel + 7) / 8;
            byte[] image_bytes = new byte[stride * drr.YSize];
            for (int i = 0; i < flat_pixels.Length; i++)
            {
                double value = flat_pixels[i];
                image_bytes[i] = Convert.ToByte(255 * ((value - drr_min) / (drr_max - drr_min)));
            }
            //build the bitmapsource.
            return BitmapSource.Create(drr.XSize, drr.YSize, 25.4 / drr.XRes, 25.4 / drr.YRes, format, null, image_bytes, stride);
        }

        private string GetGantry(Beam beam)
        {
            //if the beam is an arc display the start and stop positions. 
            if (beam.Technique.Id.Contains("ARC"))
            {
                return $"{beam.ControlPoints.First().GantryAngle:F1} {(beam.GantryDirection == GantryDirection.Clockwise ? "CW" : "CCW")} {beam.ControlPoints.Last().GantryAngle:F1}";
            }
            return $"{beam.ControlPoints.First().GantryAngle:F1}";//Gantry angle in degrees.
        }

        private string GetIsocenter(PlanSetup plan, Beam beam)
        {
            //Get the isocenter of the beam relative to the user origin.
            var iso = beam.IsocenterPosition;//VVector
            //DICOMToUser converts DICOM Scale to have origin at the user origin in user coordinate system.
            var userIso = plan.StructureSet.Image.DicomToUser(iso, plan);
            return $"({userIso.x / 10.0:F1}, {userIso.y / 10.0:F1}, {userIso.z / 10.0:F1})";//cm
        }
    }
}
