using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using Assistment.Extensions;
using Assistment.Texts;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.LinearAlgebra;
using System.Runtime.InteropServices;
//using Assistment.Drawing;

using Werwolf.Generating;
using Werwolf.Karten;
using Werwolf.Forms;
using Werwolf.Inhalt;

namespace Werwolf
{
    static class Program
    {
        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        // According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware);

            Application.Run(new StartForm<Universe>());
        }
    }
}
