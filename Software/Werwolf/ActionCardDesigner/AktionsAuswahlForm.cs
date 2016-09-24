using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Werwolf.Karten;
using Werwolf.Forms;
using Werwolf.Inhalt;

namespace ActionCardDesigner
{
    public class AktionsAuswahlForm<T> : ElementAuswahlForm<T> where T : XmlElement, new()
    {
        public AktionsAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge, T Element, ViewBox ViewBox, bool BearbeitungErlaubt)
            : base(Karte, ElementMenge, Element, ViewBox, BearbeitungErlaubt)
        {
            
        }

        protected override PreForm<T> GetPreform()
        {
            switch (typeof(T).Name)
            {
                case "AktionsKarte":
                    return new TitelDarstellungForm(Karte) as PreForm<T>;
                case "AktionsDeck":
                    return new AktionsDeckForm(Karte) as PreForm<T>;
                default:
                    return base.GetPreform();
            }
        }
    }
}
