using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using System.Drawing;

namespace Werwolf.Karten.CyberAktion
{
    public class CyberWaffenHeader : CyberAktionHeader
    {
        public CyberWaffenHeader(Karte Karte, float Ppm) 
            : base(Karte, Ppm, 5)
        {
            OnKarteChanged();
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null|| Lines == null) return;
            Text[] schaden = Karte.Effekt.ProduceTexts(Font);
            Text[] gesinnung = Karte.Gesinnung.Aufgabe.ProduceTexts(Font);
            this[2, 1] = schaden.Length > 0 ? schaden[0].Geometry(InnenRadius.mul(Faktor)) : null;
            this[3, 0] = gesinnung.Length > 0 ? gesinnung[0].Geometry(InnenRadius.mul(Faktor)) : null;
            this[3, 1] = gesinnung.Length > 1 ? gesinnung[1].Geometry(InnenRadius.mul(Faktor)) : null;
            this[4, 0] = GetZielSicherheiten().Geometry(InnenRadius.mul(Faktor));
        }

        public DrawBox GetZielBox()
        {
            DrawBox inter = GetZielSicherheiten();
            xFont small = Font.GetFontOfSize(8);
            FixedBox fBox = new FixedBox(new SizeF(InnenBox.Width, small.GetZeilenabstand()*1.25f),true, true, inter);
            fBox.Alignment = new SizeF(0.5f, 0.5f);
            return fBox.Colorize(Brushes.White);//new Pen(Color.Black,ppm/5),.Geometry(InnenRadius.mul(Faktor)).Colorize(Darstellung.RandFarbe.ToPen(Darstellung.Rand.Width),Darstellung.Farbe.ToBrush());
        }

        private Text GetZielSicherheiten()
        {
            xFont small = Font.GetFontOfSize(8);
            Text text = new Text("", small);
            //text.add(new WolfTextBild(Layout.ZielSicherheitenSchutze, Font));
            string pure = Karte.ZielSicherheiten.Replace(" ", "");
            for (int i = 0; i < pure.Length; i++)
            {
                if (i % 4 == 0 && i > 0)
                    text.AddWhitespace(Font.GetWhitespace() * 1);
                text.Add(GetZielSicherheit(pure[i], small));
            }
            return text;
        }
        private DrawBox GetZielSicherheit(char a, xFont Font)
        {
            TextBild tb;
            if ('0' <= a && a <= '9')
                tb = Layout.ZielSicherheiten[a - '0' + 9];
            else if ('a' <= a && a <= 'i')
                tb = Layout.ZielSicherheiten['a' - a + 8];
            else if (a == '+')
                tb = Layout.ZielSicherheiten[10];
            else if (a == '-')
                tb = Layout.ZielSicherheiten[8];
            else
                return new ImageBox(Font.YMass('_') * Settings.NotFoundImage.Width / Settings.NotFoundImage.Height, Settings.NotFoundImage);

            return new WolfTextBild(tb, Font);
        }
    }
}
