using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Werwolf.Inhalt.Data
{
    public class Konflikt
    {
        public enum Art
        {
            /// <summary>
            /// Die Quelle existiert nicht, nichts sollte vorgenommen werden.
            /// </summary>
            BildLeer,
            /// <summary>
            /// Die Quelle existiert, das Ziel ist leer, der Kopiervorgang kann vorgenommen werden.
            /// </summary>
            KeinKonflikt,
            /// <summary>
            /// Quelle und Ziel sind identisch, nichts sollte vorgenommen werden.
            /// </summary>
            QuelleUndZielIdentisch,
            /// <summary>
            /// Im Ziel ist eine Datei mit demselben Inhalt, nicht sollte vorgenommen werden.
            /// </summary>
            ZielMitGleicherDateiBesetzt,
            /// <summary>
            /// Im Ziel ist eine Datei mit verschiedenem Inhalt, User muss gefragt werden.
            /// </summary>
            ZielMitVerschiedenerDateiBesetzt
        }
        public enum Losung
        {
            /// <summary>
            /// Es wurde keine Lösung festgelegt
            /// </summary>
            Keine,
            /// <summary>
            /// Die Datei im Ziel wird ersetzt
            /// </summary>
            Ersetzen,
            /// <summary>
            /// Die Datei im Ziel wird nicht ersetzt, und die Quelle wird nicht rüberkopiert.
            /// </summary>
            NichtErsetzen,
            /// <summary>
            /// Die Datei in der Quelle wird mit einem neuen Namen rüberkopiert.
            /// </summary>
            Umbennen
        }

        public Art FehlerArt { get; set; }
        public Losung LosungArt { get; set; }
        public Bild Bild { get; set; }
        public string SourceFile { get; set; }
        public string DestinyFile { get; set; }

        public Konflikt(Bild Bild, string SourceFile, string DestinyFile)
        {
            this.Bild = Bild;
            this.SourceFile = SourceFile;
            this.DestinyFile = DestinyFile;
            this.FehlerArt = Evaluate(SourceFile, DestinyFile);
            this.LosungArt = GetStandardLosung(FehlerArt);
        }

        public Losung GetStandardLosung(Art fehlerArt)
        {
            switch (fehlerArt)
            {
                case Art.BildLeer:
                case Art.QuelleUndZielIdentisch:
                case Art.ZielMitGleicherDateiBesetzt:
                    return Losung.NichtErsetzen;
                case Art.KeinKonflikt:
                    return Losung.Ersetzen;
                case Art.ZielMitVerschiedenerDateiBesetzt:
                    return Losung.Keine;
                default:
                    throw new NotImplementedException();
            }
        }
        public Art Evaluate(string SourceFile, string DestinyFile)
        {
            if (!File.Exists(SourceFile))
                return Art.BildLeer;
            else if (!File.Exists(DestinyFile))
                return Art.KeinKonflikt;
            else if (AreEqualFilenames(SourceFile, DestinyFile))
                return Art.QuelleUndZielIdentisch;
            else if (AreEqualFiles(SourceFile, DestinyFile))
                return Art.ZielMitGleicherDateiBesetzt;
            else
                return Art.ZielMitVerschiedenerDateiBesetzt;
        }
        public long GetSize(string file)
        {
            return new FileInfo(file).Length;
        }
        public bool AreEqualFilenames(string Filename1, string Filename2)
        {
            return new Uri(Filename1).LocalPath.Equals(new Uri(Filename2).LocalPath);
        }
        public unsafe bool AreEqualFiles(string file1, string file2)
        {
            if (GetSize(SourceFile) != GetSize(DestinyFile))
                return false;

            byte[] array1 = File.ReadAllBytes(file1);
            byte[] array2 = File.ReadAllBytes(file2);
            fixed (byte* p1 = array1, p2 = array2)
            {
                long* l1 = (long*)p1, l2 = (long*)p2;
                int i = 0;
                for (; i < array1.Length / sizeof(long); i++)
                    if (*(l1 + i) != *(l2 + i))
                        return false;
                for (i *= sizeof(long); i < array1.Length; i++)
                    if (*(p1 + i) != *(p2 + i))
                        return false;
            }
            return true;
        }
    }
}
