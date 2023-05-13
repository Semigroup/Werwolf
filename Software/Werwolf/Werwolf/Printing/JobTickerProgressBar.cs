using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Threading;
using System.Diagnostics;

using Assistment.Xml;
using Assistment.Extensions;
using Assistment.Mathematik;
using Assistment.PDF;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Printing
{
    public class JobTickerProgressBar : IJobTicker
    {
       public ProgressBar ProgressBar { get; set; }

        public JobTickerProgressBar(ProgressBar ProgressBar)
        {
            this.ProgressBar = ProgressBar;
        }

        public void Exited(int IDJob, int ExitCode)
        {
            if (ProgressBar != null)
            {
                if (ExitCode != 0)
                    ProgressBar.Invoke((MethodInvoker)delegate { ProgressBar.ForeColor = Color.Red; });
                ProgressBar.Invoke((MethodInvoker)delegate { ProgressBar.PerformStep(); });
            }
        }

        public void Reset(int NumberOfJobs)
        {
            if (ProgressBar != null)
                ProgressBar.Invoke((MethodInvoker)delegate
                {
                    ProgressBar.Value = 0;
                    ProgressBar.Maximum = NumberOfJobs;
                });
        }
    }
}
