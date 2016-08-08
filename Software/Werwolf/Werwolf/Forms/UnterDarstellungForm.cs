using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public partial class UnterDarstellungForm<T> : Form where T : UnterDarstellung
    {
        public UnterDarstellungForm()
        {
            InitializeComponent();
        }
    }
}
