using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ActionCardDesigner
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
            Application.Run(new AktionsStartForm());

            //AktionsUniverse au = new AktionsUniverse();
            //AktionsKarte ak = au.AktionsKarten.Standard;
            //ak.TitelDarstellung.Font = new System.Drawing.Font("Calibri", 11);
            //StandardAktionsKarte sak = new StandardAktionsKarte(ak, 10);
            //ak.Initiative = 0.15f;
            //ak.Storung = 5;
            //ak.Felder = 10;
            //ak.ReichweiteMin = 1;
            //ak.ReichweiteMax = 12;
            //sak.setup(0);
            //sak.createImage("test");
        }
    }
}
