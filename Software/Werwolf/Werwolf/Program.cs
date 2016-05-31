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

using Werwolf.Karten;

namespace Werwolf
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Text t = new Text();
            t.preferedFont = new FontMeasurer("Calibri", 11);
            #region Text
            t.addRegex(@"This is the Golden Age of Grotesque,
Be prepared for what you may find.");
            #endregion
            t.alignment = 0.5f;
            t.addWhitespace(400, 0, true);
            t.addWhitespace(0, 0, false);

            Weg y = x => new PointF();//new PointF((float)Math.Cos(x * Math.PI * 10) * 20, 25 + 25 * (float)Math.Sin(x * Math.PI * 2 * 30));

            float RandHohe = 50;

            CString cs = new CString();
            cs.add(new RunderTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new StachelTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new ZahnTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new WellenTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new SagezahnTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new VierStufenTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new KonigTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new ChaosTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red));
            cs.add(new KreuzTitel(t.clone(), RandHohe, Pens.Black, Brushes.Red)); 
            
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
