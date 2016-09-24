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
//using Assistment.Drawing;

using Werwolf.Generating;
using Werwolf.Karten;
using Werwolf.Forms;
using Werwolf.Inhalt;

namespace Werwolf
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartForm<Universe>());
        }
    }
}
