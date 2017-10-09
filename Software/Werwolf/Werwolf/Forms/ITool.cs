using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using System.Windows.Forms;

namespace Werwolf.Forms
{
    public interface ITool
    {
        string ToolDescription { get;  }
        DialogResult EditUniverse(Universe universe);
    }
}
