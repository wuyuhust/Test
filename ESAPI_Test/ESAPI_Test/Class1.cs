using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS

{
    public class Script
    {
        public Script()
        {
        }

        public void Execute(ScriptContext context, Window window)
        {
            string name = context.Patient.LastName + ", " + context.Patient.FirstName;
            MessageBox.Show(name);
        }
    }
}
