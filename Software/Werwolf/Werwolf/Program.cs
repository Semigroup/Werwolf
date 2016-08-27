using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using Assistment.Extensions;
using Assistment.Texts;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.LinearAlgebra;
//using Assistment.Drawing;

using Werwolf.Generating;
using Werwolf.Karten;
using Werwolf.Forms;
using Werwolf.Inhalt;

namespace Werwolf
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Universe Universe = new Universe();
            Universe.Root(@"D:\CSArbeiten\Github\Werwolf\WH40K\");
            Universe.Save();

            //IKartenmacher kk = new KartenPDFMacher();
            //kk.MakeKarten(Universe, @"D:\CSArbeiten\Github\Werwolf\WH40K\Karten\", 10);
            //PDF();
            //CollectBilder(@"D:\CSArbeiten\Github\Werwolf\WH40K\Bilder\", @"D:\CSArbeiten\Github\Werwolf\WH40K\Universe.xml");
        }

        public static void PDF()
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream("test.pdf", FileMode.Create));
            document.Open();
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("asdad.png");
            float w = img.ScaledWidth;
            float h = img.ScaledHeight;
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            cb.SaveState();
            //cb.Circle(260, 700, 70);
            cb.Rectangle(0, 700, 1000, 100);
            cb.Clip();
            cb.NewPath();
            cb.AddImage(img, w, 0, 0, h, 36, 620);
            cb.RestoreState();

            document.Close();
        }

        public static void CollectBilder(string path, string UniversePath)
        {
            Universe Universe = new Universe(UniversePath);
            ElementMenge<Bild> Bilder = Universe.Bilder;
            float width = Universe.HintergrundDarstellungen.Standard.Size.Width;
            foreach (var item in Directory.EnumerateFiles(path, "*.jpg").Concat(Directory.EnumerateFiles(path, "*.png")))
            {
                Bild b = new Bild();
                b.FilePath = item;
                b.Name = Path.GetFileNameWithoutExtension(item);
                b.Size = new SizeF(width, width / ((SizeF)b.Image.Size).ratio());
                b.Zentrum = new PointF(0.5f, 0.5f);
                Bilder.Add(b);
            }
            Bilder.Save();
        }

        public static void TestLayer()
        {
            //int c = 100;
            //Bitmap b = new Bitmap(c * 20, c * 28);
            //Graphics g = b.GetHighGraphics();
            //g.Clear(Color.Black);

            //PointF Mittelpunkt = new PointF(10 * c, 14 * c);
            //OrientierbarerWeg ow = OrientierbarerWeg.Spirale(c * 10, 10);
            //ow += Mittelpunkt;


            //Schema s = Schema.WithTeeth(1);
            //s.sampleRate = 10;
            //s.strings *= 10;
            //s.hohe = t => 100;
            //s.burst = c;

            //Shadex.chaosWeg(g, ow, s);

            //g.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), 0, 0, 20 * c, 28 * c);

            //b.Save("test.png");


            LayerBox lb = new LayerBox();
            float w = 1000;
            lb.addImage(Image.FromFile("test.png"), w, w * 1.4f);

            Text te = new Text();
            te.preferedFont = new FontMeasurer("Exocet", 30);
            te.alignment = 0.5f;
            te.addRegex(@"Gutschein für eine Rollenspielrunde");
            te.addAbsatz();
            te.addWhitespace(1000, 100, true);

            te.preferedFont = new FontMeasurer("Calibri", 20);
            te.addRegex(@"Hey, David! Alles Gute zum Geburtstag. Als Geschenk erhätst Du von mir einen Gutschein für ein Rollenspielszenario, das wir in den Semesterferien spielen können, sobald die Klausurenphase bei allen Beteiligten vorbei ist.
Ich kann jetzt noch nicht sagen, wie das Szenario genau ausschauen wird, weil das alles top secret ist, aber hier ein paar Eckdaten:");
            te.addAbsatz();

            DrawList dl = new DrawList();
            dl.preferedFont = new FontMeasurer("Calibri", 20);
            dl.add(new Text(@"- Ausgelegt soll das ganze auf einen Abend sein.
Eventuell sogar, falls sich genügend Menschenmaterial findet, einen Indoor-Abend und einen LARP-Teil an einem anderen Tag, oder vielleicht sogar nur eine LARP-Session.", dl.preferedFont));
            dl.addWhitespace(0, 10, false);
            dl.add(new Text(@"- Die Spieler werden voraussichtlich aus Dir und vier weiteren Mitspielern bestehen.", dl.preferedFont));
            dl.addWhitespace(0, 10, false);
            dl.add(new Text(@"- Es soll möglichst surreal und Mistery mit ganz viel Pseudo-Kafka- und Pseudo-Lynch-Shit und deutlich weniger Logik sein.", dl.preferedFont));
            dl.addWhitespace(0, 10, false);
            dl.add(new Text(@"- Es soll (nahezu) keine Kämpfe oder spieltechnische Regeln geben.", dl.preferedFont));
            dl.addWhitespace(0, 10, false);
            dl.add(new Text(@"- Es wird vermutlich in dieser Zeit in Karlsruhe spielen, in einer Welt, in der es (nahezu) keine Magie oder Ähnliches gibt.", dl.preferedFont));
            dl.addWhitespace(0, 10, false);
            dl.addWhitespace(800f);

            te.add(dl.Geometry(100, 10, 100, 10));
            te.addAbsatz();
            te.addRegex(@"Alles Gute auf Deinem weiteren Lebensweg und Deinen Welteroberungsplänen wünscht");
            te.addAbsatz();
            te.addWhitespace(10, 20, true);
            te.addWhitespace(10, 20, true);
            te.preferedFont = new FontMeasurer("PlainGermanica", 30);
            te.addWort("Dein Ruestue");

            lb.add(te.Geometry(30));
            lb.createPDF("Karte");

        }
        public static void TestTitel()
        {
            Text t = new Text();
            t.preferedFont = new FontMeasurer("Exocet", 12);
            #region Text
            t.addWort(@"Test  -- Dankeschön");
            #endregion
            t.alignment = 0.5f;

            FontMeasurer f = new FontMeasurer("Exocet", 12);

            float RandHohe = 10;

            Pen p = new Pen(Color.Black, 1);
            p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            Titel[] ts = new Titel[] {
            new RunderTitel(new Text("RunderTitel",f), RandHohe, p, Brushes.Red),
            new StachelTitel(new Text("StachelTitel",f), RandHohe, p, Brushes.Red),
            new ZahnTitel(new Text("ZahnTitel",f), RandHohe, p, Brushes.Red),
            new WellenTitel(new Text("WellenTitel",f), RandHohe, p, Brushes.Red),
            new SagezahnTitel(new Text("SagezahnTitel",f), RandHohe, p, Brushes.Red),
            new VierStufenTitel(new Text("VierStufenTitel",f), RandHohe, p, Brushes.Red),
            new KonigTitel(new Text("KonigTitel",f), RandHohe, p, Brushes.Red),
            new ChaosTitel(new Text("ChaosTitel",f), RandHohe, p, Brushes.Red),
            new KreuzTitel(new Text("KreuzTitel",f), RandHohe, p, Brushes.Red),
            new TriskelenTitel(new Text("TriskelenTitel",f), RandHohe, p, Brushes.Red),
            new PikTitel(new Text("PikTitel",f), RandHohe, p, Brushes.Red),
            new BlitzTitel(new Text("BlitzTitel",f), RandHohe, p, Brushes.Red)
            };
            CString cs = new CString();
            foreach (Titel tit in ts)
            {
                DrawList l = new DrawList();
                for (int i = 1; i <= 10; i++)
                {
                    Titel tt = (Titel)tit.clone();
                    tt.Scaling = i;
                    l.add(tt);
                }
                cs.add(l);
            }

            //cs.createImage("test", 1000, float.MaxValue);
            cs.Geometry(50).createPDF("test");
        }
        public static void MakeArtist(string directory)
        {
            //Image img = Image.FromFile("C:/Users/msi/Desktop/Bild.jpg");
            //Image img2 = Image.FromFile("C:/Users/msi/Desktop/Bild.jpg");

            //MessageBox.Show(img.PropertyIdList.Length + " , " + img.PropertyItems.Length);

            //PropertyItem p = img2.GetPropertyItem(0x5091);
            //p.Id = 0x013B;
            //p.Len = 64;
            //p.Type = 2;
            //Encoding enc = Encoding.ASCII;
            //p.Value = enc.GetBytes("Boris");

            //img2.Dispose();

            //img.SetPropertyItem(p);

            //string s = "";
            //for (int i = 0; i < img.PropertyIdList.Length; i++)
            //{
            //    s += img.PropertyIdList[i];
            //    s += " -> " + img.PropertyItems[i].Id.ToString("x") + ", " + img.PropertyItems[i].Len + ", " + img.PropertyItems[i].Type + ", " + img.PropertyItems[i].Value.Length;
            //    s += Environment.NewLine;
            //}
            //MessageBox.Show(s);
            //MakeArtist("D:/CSArbeiten/Github/Werwolf/Bilder");

            Image img = Image.FromFile("C:/Users/msi/Desktop/Bild.jpg");

            PropertyItem p = img.GetPropertyItem(0x5091);
            p.Id = 0x013B;
            p.Type = 2;
            Encoding enc = Encoding.ASCII;
            p.Value = enc.GetBytes("Boris");
            img.Dispose();

            foreach (var item in Directory.GetDirectories(directory))
            {
                string artist = ((item + "/").Ordner());
                p.Value = enc.GetBytes(artist);
                foreach (var file in Directory.GetFiles(item))
                {
                    FileStream fs = new FileStream(file, FileMode.Open);
                    img = new Bitmap(fs);
                    fs.Close();
                    img.SetPropertyItem(p);
                    img.Save(file);
                }
            }

            foreach (var item in Directory.GetDirectories(directory))
            {
                string s = "";
                foreach (var file in Directory.GetFiles(item))
                {
                    img = Image.FromFile(file);
                    s += file + ": " + enc.GetString(img.GetPropertyItem(0x013B).Value);
                    s += Environment.NewLine;
                }
                MessageBox.Show(s);
            }
        }
    }
}
