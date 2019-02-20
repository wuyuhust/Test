using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS

{
    public class Script
    {
        public Script()
        {
        }

        public void Execute(ScriptContext cont/*, Window window*/)
        {
            string msg = "Patient Information \n";
            msg += "Patient Name: " + cont.Patient.LastName + ", " + cont.Patient.FirstName + "\n";
            msg += "DOB: " + cont.Patient.DateOfBirth.ToString() + "\n";
            msg += "Hospital: " + cont.Patient.Hospital + "\n";
            msg += "Oncologist: " + cont.Patient.PrimaryOncologistId + "\n\n";

            MessageBox.Show(msg);

            //msg += "\nCourse Information: \n";
            //msg += "Course: " + cont.Course.Id + "\n";
            //msg += "Plans: " + string.Join("\n", cont.Course.PlanSetups.Select(x => x.Id)) + "\n";

            //MessageBox.Show(msg);
            msg = "";

            msg += "\nPlan Information: \n";
            msg += "Plan ID: " + cont.PlanSetup.Id + "\n";
            msg += "Plan Approval: " + cont.PlanSetup.ApprovalStatus.ToString() + "\n";
            msg += "Plan Creation Date: " + cont.PlanSetup.CreationDateTime.ToString() + "\n";
            msg += "Plan Target Volume: " + cont.PlanSetup.TargetVolumeID + "\n";
            msg += "Prescribed Dose Value: " + cont.PlanSetup.TotalPrescribedDose.ValueAsString + "\n";
            msg += "Plan Max Dose: " + cont.PlanSetup.Dose.DoseMax3D.ValueAsString + "\n";
            msg += "Dose Unit: " + cont.PlanSetup.Dose.DoseMax3D.UnitAsString + "\n";
            msg += "Max Dose Location: (" +
                cont.PlanSetup.Dose.DoseMax3DLocation.x.ToString("F2") + ", " +
                cont.PlanSetup.Dose.DoseMax3DLocation.y.ToString("F2") + ", " +
                cont.PlanSetup.Dose.DoseMax3DLocation.z.ToString("F2") +
                ") \n";

            // dose inside target volume
            Structure targetVolume = null;

            if (!string.IsNullOrEmpty(cont.PlanSetup.TargetVolumeID))
                targetVolume = cont.PlanSetup.StructureSet.Structures.FirstOrDefault(s => s.Id.Equals(cont.PlanSetup.TargetVolumeID));
            else
                targetVolume = cont.PlanSetup.StructureSet.Structures.FirstOrDefault(s => s.Id.ToUpper().Contains("PTV"));
            if (targetVolume != null)
            {
                VVector ct_Volume = targetVolume.CenterPoint;
                VVector maxDoseLocation = cont.PlanSetup.Dose.DoseMax3DLocation;
                if (targetVolume.HasSegment && targetVolume.IsPointInsideSegment(maxDoseLocation))
                    msg += "Dose is inside target volume. \n";
                else
                    msg += "Dose is NOT inside target volume. \n";
            }

            MessageBox.Show(msg);
            msg = "";

            msg += "\nInformation About Structure Set: \n";
            msg += "Number of Structures is: " + cont.PlanSetup.StructureSet.Structures.Count().ToString();
            foreach (Structure s in cont.PlanSetup.StructureSet.Structures)
            {
                msg += "    --" + s.Id + ": " + s.Volume.ToString("F2") + "cc\n";
            }

            MessageBox.Show(msg);
            msg = "";
            msg += "\nField Information: \n";
            msg += "Number of Fields is: " + cont.PlanSetup.Beams.Count().ToString() + "\n";
            foreach (Beam beam in cont.PlanSetup.Beams)
            {
                msg += "    --" + beam.Id + ": " + beam.Meterset.Value.ToString() + "MU\n";
            }

            MessageBox.Show(msg);
        }
    }
}
