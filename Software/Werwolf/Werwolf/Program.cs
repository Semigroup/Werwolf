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
using Assistment.Drawing;

using Werwolf.Karten;

namespace Werwolf
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //int n = 5;
            //float Radius = 500;
            //int Breite = 2500;
            //float Dicke = 100;
            //OrientierbarerWeg end;
            //PointF M = new PointF(Breite / 2, Breite / 2);

            //end = OrientierbarerWeg.Triskele(Radius, n, Dicke);

            //end += M;
            //end.Trim(0.01f, 0.99f).print(Breite, Breite, 10);
            TestTitel();

        }

        public static void TestTitel()
        {
            Text t = new Text();
            t.preferedFont = new FontMeasurer("Exocet", 56);
            #region Text
            t.addRegex(@"Can you see me going down
I am screaming out loud");
            #endregion
            t.alignment = 0.5f;
            t.addWhitespace(1600, 0, true);
            t.addWhitespace(0, 0, false);

            Weg y = x => new PointF();//new PointF((float)Math.Cos(x * Math.PI * 10) * 20, 25 + 25 * (float)Math.Sin(x * Math.PI * 2 * 30));

            float RandHohe = 50;

            Pen p = new Pen(Color.Black, 4);
            p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            CString cs = new CString();
            cs.add(new RunderTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new StachelTitel(t.clone(), RandHohe,p, Brushes.Red));
            cs.add(new ZahnTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new WellenTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new SagezahnTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new VierStufenTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new KonigTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new ChaosTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new KreuzTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new TriskelenTitel(t.clone(), RandHohe, p, Brushes.Red));
            cs.add(new PikTitel(t.clone(), RandHohe, p, Brushes.Red));

            cs.createImage("test", 1000, float.MaxValue);
            cs.createPDF("test");
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
