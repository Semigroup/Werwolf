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
    public interface IJobTicker
    {
        /// <summary>
        /// Setzt den Counter auf Null und gibt die neue Zahl an Jobs mit
        /// </summary>
        void Reset(int NumberOfJobs);
        /// <summary>
        /// Teilt mit, dass Job mit ID beendet wurde
        /// </summary>
        /// <param name="IDJob"></param>
        void Exited(int IDJob, int ExitCode);
    }
}
