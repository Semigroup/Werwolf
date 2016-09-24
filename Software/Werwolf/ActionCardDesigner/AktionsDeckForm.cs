using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.form;

using Werwolf.Forms;
using Werwolf.Inhalt;

namespace ActionCardDesigner
{
    public class AktionsDeckForm : PreForm<AktionsDeck>
    {
        public new AktionsUniverse Universe { get { return base.Universe as AktionsUniverse; } }

        public AktionsDeckForm(Karte Karte)
            : base(Karte, new AktionsViewDeck())
        {

        }
        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
            UpdatingWerteListe = true;
            foreach (var item in Universe.AktionsKarten)
            {
                IntPlusMinusBox ipmBox = new IntPlusMinusBox();
                ipmBox.UserValueMinimum = 0;
                WerteListe.AddWertePaar<int>(ipmBox, 0, item.Value.Name);
            }
            this.WerteListe.WertChanged += new WertEventHandler(WerteListe_WertChanged);
            WerteListe.Setup();
            UpdatingWerteListe = false;
        }

        void WerteListe_WertChanged(object sender, WertEventArgs e)
        {
            if (UpdatingWerteListe)
                return;
            if (e.Name == "Name")
                Element.Name = Element.Schreibname = e.Value as string;
            else
            {
                AktionsKarte k = Universe.AktionsKarten[e.Name];
                Element.SetKarte(k, (int)e.Value);
            }
        }
        public override void UpdateElement()
        {
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            UpdatingWerteListe = true;
            foreach (var item in Universe.AktionsKarten.Values)
                WerteListe.SetValue(item.Name, Element[item]);
            UpdatingWerteListe = false;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            ViewBox.Width = ClientSize.Width * 2 / 3;
            ViewBox.OnKarteChanged();

            OkButton.Location = new Point(ViewBox.Right + 20, ClientSize.Height - 50);
            AbbrechenButton.Location = new Point(OkButton.Right + 20, OkButton.Top);

            WerteListe.Location = new Point(ViewBox.Right, 0);
            WerteListe.Size = new Size(ClientSize.Width / 3, ClientSize.Height - 70);
        }
    }
}
