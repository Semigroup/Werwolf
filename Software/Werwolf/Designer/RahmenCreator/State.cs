﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.Geometries.Extensions;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using Assistment.Drawing;

namespace Designer.RahmenCreator
{
    public class State
    {
        /// <summary>
        /// Pixel Size of Bitmap
        /// </summary>
        public Size Size;

        public float PPM;
        public string UniverseName;
        public string HintergrundBildName;
        public string HintergrundDarstellungName;
        public string Name;
        public float MarginLeft, MarginRight, MarginTop, MarginBottom;
        public float Radius;
        public float FragmentDicke;
        public int FragmentZahl;
        public Fragments.Style FragmentStyle;
        public int Samples;
        public bool InvertLeft;
        public bool InvertRight;
        public float Shift;

        public Color PenColor;
        public float PenWidth;

        public RectangleF TextBox;
        public PointF TextBoxShadowOffset;

        public bool TextBoxActive;
        public float TextBoxRadius;

        [XmlIgnore()]
        public Color TextColor;
        [XmlElement(ElementName = "TextColor")]
        public String TextColor_XmlSurrogate
        {
            get => TextColor.ToHexString();
            set { TextColor = value.ToColor(); }
        }
        [XmlIgnore()]
        public Color TextShadowColor;
        [XmlElement(ElementName = "TextShadowColor")]
        public String TextShadowColor_XmlSurrogate
        {
            get => TextShadowColor.ToHexString();
            set { TextShadowColor = value.ToColor(); }
        }
    }
}
