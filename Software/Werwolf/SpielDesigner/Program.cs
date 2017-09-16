using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using Werwolf.Inhalt;
using Werwolf.Forms;
using Assistment.Testing;
using Assistment.Texts;
using Assistment.Texts.Paper;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Werwolf.Karten;

namespace SpielDesigner
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //SizeF Size = new SizeF(63/2f, 89.1f).mul(WolfBox.Faktor);
            //CardSheet cs = new CardSheet(6, 3, Size);

            //string path = @"D:\Rollenspiel\Trilogie-der-Neusten-Zeit\CyberSage\Gimp\Charaktere\";
            //string[] names = new string[] {
            //    "ArthurMcPlasma", "Corbin","HermannStrakker", "Promethon", "Kerberus",
            //    "HalbNaziDieHard","HalbNaziSöldner","HalbNaziTank","HalbNaziHacker",
            //    "Assistent", "Schrödinger","Jarlson", "Megaira","MeshinuaShykrim",
            //    "SystemloserDieHard","SystemloserSöldner1","SystemloserSöldner2","SystemloseHackerin"
            //};
            //for (int i = 0; i < 18; i++)
            //    cs.add(new ImageBox(Size.Width, Image.FromFile(path + names[i] + "KleinVorderseite.png")));
            //foreach (int i in new int[] { 5,4,3, 2, 1, 0,
            //    11,10,9,8,7,6,
            //    17,16,15,14, 13, 12 })
            //    cs.add(new ImageBox(Size.Width, Image.FromFile(path + names[i] + "KleinRückseite.png")));
            //cs.createPDF("CharaktereKlein");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartForm<Universe>());
        }
    }
}
