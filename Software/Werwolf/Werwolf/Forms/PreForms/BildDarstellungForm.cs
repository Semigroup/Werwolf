using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public class BildDarstellungForm : DarstellungForm<BildDarstellung>
    {
        public BildDarstellungForm( Karte Karte)
            : base( Karte)
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
        }
    }
}
