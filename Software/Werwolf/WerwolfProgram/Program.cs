using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Werwolf.Forms;
using Werwolf.Inhalt;
using Translation.PDFWorkarounds;
using Designer;
using System.Runtime.InteropServices;

namespace Translation
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

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware);
            //(int)DpiAwareness.PerMonitorAware makes the line height of fonts higher. Why?
            //Has been fixed by changes in FontGraphicsMeasurer in Assistment.Texts

            Application.Run(new StartForm<Universe>(new TranslatingTool(),
                new HintergrundTool()));//, new ProduktionSteik()
        }
    }
}
