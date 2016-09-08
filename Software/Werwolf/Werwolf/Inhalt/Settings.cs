using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

using Assistment.Xml;
using Assistment.Extensions;

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

        static Settings()
        {
            current = Directory.GetCurrentDirectory();
            path = Path.Combine(current, "Ressourcen/Settings.xml");

            XmlReader reader = XmlReader.Create(path);
            reader.Next();
            if (reader.Name != "Settings")
                throw new NotImplementedException();
            MaximumNumberOfCores = reader.getInt("MaximumNumberOfCores");
            DelayTime = reader.getInt("DelayTime");
            SleepTime = reader.getInt("SleepTime");
            MaximumKarteSize = reader.getSizeF("MaximumKarteSize");
            MaximumImageArea = reader.getInt("MaximumImageArea");
            WolfBoxFaktor = reader.getFloat("WolfBoxFaktor");
            MaximumPpm = reader.getFloat("MaximumPpm");
            errorImagePath = reader.getString("ErrorImagePath");
            ViewPpm = reader.getFloat("ViewPpm");

            using (FileStream fs = new FileStream(ErrorImagePath, FileMode.Open))
            {
                ErrorImage = Image.FromStream(fs);
                fs.Close();
            }

            reader.Close();
        }
        public static void Save()
        {
            XmlWriter writer = XmlWriter.Create(path);
            writer.WriteStartDocument();
            writer.WriteStartElement("Settings");
            writer.writeInt("MaximumNumberOfCores", MaximumNumberOfCores);
            writer.writeInt("DelayTime", DelayTime);
            writer.writeInt("SleepTime", SleepTime);
            writer.writeSize("MaximumKarteSize", MaximumKarteSize);
            writer.writeInt("MaximumImageArea", MaximumImageArea);
            writer.writeFloat("WolfBoxFaktor", WolfBoxFaktor);
            writer.writeFloat("MaximumPpm", MaximumPpm);
            writer.writeFloat("ViewPpm", ViewPpm);

            ErrorImage.ForcedSave(ErrorImagePath);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
