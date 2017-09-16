using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Werwolf.Inhalt.Data;

namespace Werwolf.Forms.Data
{
    public partial class KonfliktForm : Form
    {
        public KonfliktForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        public void SetKonflikte(IEnumerable<Konflikt> Konflikte)
        {
            foreach (var item in Konflikte)
            {
                KonfliktFeld kf = new KonfliktFeld();
                kf.SetKonflikt(this, item);
                scrollList1.AddControl(kf);
            }
        }
        private void AbbrechenButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void scrollList1_Load(object sender, EventArgs e)
        {
            scrollList1.SetUp();
        }
        public void RemoveKonfliktFeld(KonfliktFeld konfliktFeld, bool useSolutionForAll)
        {
            scrollList1.ControlList.Remove(konfliktFeld);
            if (useSolutionForAll)
            {
                foreach (KonfliktFeld item in scrollList1.ControlList)
                    item.Konflikt.LosungArt = konfliktFeld.Konflikt.LosungArt;
                scrollList1.ControlList.Clear();
            }
          
            scrollList1.SetUp();
            if (scrollList1.ControlList.Count == 0)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
