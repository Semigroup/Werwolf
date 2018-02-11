using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Werwolf.Forms;
using Werwolf.Inhalt;
using Translation.PDFWorkarounds;
using Designer;


namespace Translation
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartForm<Universe>(new TranslatingTool(),
                new HintergrundTool()));//, new ProduktionSteik()
        }
    }
}
