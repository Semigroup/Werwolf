using System.Drawing;
using System.Linq;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Karten.Alchemie
{
    public class AlchemieSubType : WolfBox
    {
        private StarBox SubType;

        public AlchemieSubType(Karte Karte, float PPm) : base(Karte, PPm)
        {
        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            RectangleF MovedInnenBox = InnenBox.move(Box.Location);
            RectangleF BottomBox = MovedInnenBox.move(InfoDarstellung.Position.mul(Faktor));
            SubType.Setup(BottomBox);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            SubType.Move(ToMove);
        }
        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible()
                && InfoDarstellung.Existiert
                && (SubType != null);
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;

            string type = Karte.Fraktion.Schreibname;
            bool containsSubtypes = Karte.Fraktion.Schreibname.Contains('-');
            if (containsSubtypes)
                type = type.Replace('-', '–');
            Text textType = new Text(type, InfoDarstellung.FontMeasurer);
            Text[] textSubTypes = Karte.Effekt.ProduceTexts(InfoDarstellung.FontMeasurer);
            foreach (var item in textSubTypes)
            {
                if (containsSubtypes)
                    textType.AddWort(", ");
                else
                    textType.AddWort(" –");
                textType.AddRange(item);
            }

            SubType = new StarBox(textType,
                InfoDarstellung.TextFarbe.ToBrush(),
                InfoDarstellung.Farbe.ToBrush(),
                InfoDarstellung.Rand.mul(Faktor).ToPointF(),
                4);
        }

        public override void Draw(DrawContext con)
        {
            SubType.Draw(con);
        }
    }
}
