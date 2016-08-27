using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Gesinnung : XmlElement
    {
        public Gesinnung()
            : base("Gesinnung", true)
        {

        }
        
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Loader.XmlReader.Next();
        }
    }
}
