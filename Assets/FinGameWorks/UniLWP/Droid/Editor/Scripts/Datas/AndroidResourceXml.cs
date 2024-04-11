using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas
{
    [XmlRoot("resources")]
    public class AndroidResourceStringXml
    {
        [XmlElement(elementName:"string", type: typeof(AndroidResourceStringEntry))]
        public List<AndroidResourceStringEntry> Strings = new List<AndroidResourceStringEntry>();
    }

    [XmlRoot("string")]
    public class AndroidResourceStringEntry
    {
        [XmlAttribute("name")]
        public String Name;
        
        [XmlText]
        public String Value;

        public AndroidResourceStringEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public AndroidResourceStringEntry() { }
    }
}