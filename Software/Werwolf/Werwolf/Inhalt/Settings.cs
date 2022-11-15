using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

using Assistment.Xml;
using Assistment.Extensions;
using ArtOfMagicCrawler;

namespace Werwolf.Inhalt
{
    public static class Settings
    {
        private static string current;
        private static string path;

        public static float WolfBoxFaktor { get; set; }
        public static int MaximumNumberOfCores { get; set; }
        public static int DelayTime { get; set; }
        public static int SleepTime { get; set; }
        public static SizeF MaximumKarteSize { get; set; }
        public static int MaximumImageArea { get; set; }
        public static float MaximumPpm { get; set; }
        public static float ViewPpm { get; set; }
        private static string errorImagePath;
        public static string ErrorImagePath
        {
            get
            {
                if (current != null && errorImagePath != null)
                    return Path.Combine(current, errorImagePath);
                else
                    return "";
            }
        }
        public static Image ErrorImage { get; set; }
        private static string notFoundImagePath;
        public static string NotFoundImagePath
        {
            get
            {
                if (current != null && notFoundImagePath != null)
                    return Path.Combine(current, notFoundImagePath);
                else
                    return "";
            }
        }
        public static Image NotFoundImage { get; set; }
        public static bool RefreshDirtyButtons { get; set; }
        public static string ArtOfMtgLibraryRoot { get; set; }
        public static ArtLibrary ArtOfMtgLibrary { get; set; }

        static Settings()
        {
            current = Directory.GetCurrentDirectory();
            path = Path.Combine(current, "Ressourcen/Settings.xml");

            XmlReader reader = XmlReader.Create(path);
            reader.Next();
            if (reader.Name != "Settings")
                throw new NotImplementedException();
            MaximumNumberOfCores = reader.GetInt("MaximumNumberOfCores");
            DelayTime = reader.GetInt("DelayTime");
            SleepTime = reader.GetInt("SleepTime");
            MaximumKarteSize = reader.GetSizeF("MaximumKarteSize");
            MaximumImageArea = reader.GetInt("MaximumImageArea");
            WolfBoxFaktor = reader.GetFloat("WolfBoxFaktor");
            MaximumPpm = reader.GetFloat("MaximumPpm");
            errorImagePath = reader.GetString("ErrorImagePath");
            notFoundImagePath = reader.GetString("NotFoundImagePath");
            ViewPpm = reader.GetFloat("ViewPpm");
            RefreshDirtyButtons = reader.GetBoolean("RefreshDirtyButtons");
            ArtOfMtgLibraryRoot = reader.GetString("ArtOfMtgLibraryRoot");
            TryLoadArtOfMtgLibrary();

            using (Image Image = Image.FromFile(ErrorImagePath))// Image.FromStream(fs))
                ErrorImage = new Bitmap(Image);
            using (Image Image = Image.FromFile(NotFoundImagePath))//Image.FromStream(fs))
                NotFoundImage = new Bitmap(Image);

            reader.Close();
        }

        public static bool TryLoadArtOfMtgLibrary()
        {
            try
            {
                ArtOfMtgLibrary = ArtLibrary.ReadLibrary(ArtOfMtgLibraryRoot);
                return true;
            }
            catch (Exception)
            {
                ArtOfMtgLibrary = null;
                return false;
            }
        }

        public static void Save()
        {
            XmlWriter writer = XmlWriter.Create(path);
            writer.WriteStartDocument();
            writer.WriteStartElement("Settings");
            writer.WriteInt("MaximumNumberOfCores", MaximumNumberOfCores);
            writer.WriteInt("DelayTime", DelayTime);
            writer.WriteInt("SleepTime", SleepTime);
            writer.WriteSize("MaximumKarteSize", MaximumKarteSize);
            writer.WriteInt("MaximumImageArea", MaximumImageArea);
            writer.WriteFloat("WolfBoxFaktor", WolfBoxFaktor);
            writer.WriteFloat("MaximumPpm", MaximumPpm);
            writer.WriteFloat("ViewPpm", ViewPpm);
            writer.WriteBoolean("RefreshDirtyButtons", RefreshDirtyButtons);
            writer.WriteAttribute("ArtOfMtgLibraryRoot", ArtOfMtgLibraryRoot);

            ErrorImage.Save(ErrorImagePath);//Forced
            writer.WriteAttribute("ErrorImagePath", errorImagePath);
            NotFoundImage.Save(NotFoundImagePath);
            writer.WriteAttribute("NotFoundImagePath", notFoundImagePath);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
