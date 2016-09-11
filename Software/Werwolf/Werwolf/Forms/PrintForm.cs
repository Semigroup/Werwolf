using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public partial class PrintForm : Form
    {
        private Universe universe;
        private Deck Deck;

        public Universe Universe
        {
            get { return universe; }
            set
            {
                universe = value;
                Deck = universe.Decks.Standard;
            }
        }

        public PrintForm()
        {
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        private void DeckButton_Click(object sender, EventArgs e)
        {

        }

        private void Drucken_Click(object sender, EventArgs e)
        {
            float ppm = floatBox1.GetValue();
            WolfSinglePaper wsp = new WolfSinglePaper(Universe, ppm);
            foreach (var item in Deck.Karten)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    wsp.TryAdd(new StandardKarte(item.Key, ppm));
                }
            }
            wsp.createPDF("test.pdf");
        }
    }
}
