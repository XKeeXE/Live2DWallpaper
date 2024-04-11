using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas
{
    [XmlRoot("manifest")]
    public class AndroidManifestXml
    {
        [XmlAttribute(attributeName:"package")]
        public string Package;
        
        [XmlElement(elementName:"application")]
        public AndroidManifestApplication Application = new AndroidManifestApplication();

        [XmlNamespaceDeclarations] 
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public AndroidManifestXml()
        {
            Package = "com.justzht.unilwp.droid.customize";
            xmlns.Add("android","http://schemas.android.com/apk/res/android");
        }

        public override string ToString()
        {
            string s = "AndroidManifestXml" + Environment.NewLine;
            s += Application == null ? "null" : Application.ToString();
            return s.Indent();
        }
    }
    
    [XmlRoot("application")]
    public class AndroidManifestApplication
    {
        [XmlElement(elementName:"meta-data", type: typeof(AndroidManifestMetaData))]
        public HashSet<AndroidManifestMetaData> MetaDatas = new HashSet<AndroidManifestMetaData>();
        
        public AndroidManifestApplication()
        {
            MetaDatas.Add(new AndroidManifestMetaData("unilwp.version.unity", UniLWP.Droid.Scripts.UniLWP.FullVersion()));
        }

        public void SetValueForMetaData(string name, string value)
        {
            AndroidManifestMetaData metaDataVersion = MetaDatas.FirstOrDefault(m => m.Name.Equals(name));
            if (metaDataVersion == null)
            {
                metaDataVersion = new AndroidManifestMetaData(name);
                MetaDatas.Add(metaDataVersion);
            }
            metaDataVersion.Value = value;
        }

        public override string ToString()
        {
            string s = "AndroidManifestApplication" + Environment.NewLine;
            foreach (AndroidManifestMetaData metaData in MetaDatas)
            {
                s += metaData.ToString().Indent();
                s += Environment.NewLine;
            }
            return s.Indent();
        }
    }

    [XmlRoot("meta-data")]
    public class AndroidManifestMetaData
    {
        protected bool Equals(AndroidManifestMetaData other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AndroidManifestMetaData) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        [XmlAttribute(AttributeName = "name", Namespace = "http://schemas.android.com/apk/res/android")]
        public string Name;
        [XmlAttribute(AttributeName = "value", Namespace = "http://schemas.android.com/apk/res/android")]
        public string Value;

        public AndroidManifestMetaData() { Name = ""; Value = ""; }
        public AndroidManifestMetaData(string name) { Name = name; Value = ""; }
        public AndroidManifestMetaData(string name, string value) { Name = name; Value = value; }

        public override string ToString()
        {
            string s = "AndroidManifestMetaData ";
            s += Name;
            s += " ";
            s += Value;
            return s.Indent();
        }
        
        public static String[] AvailableMetadataKeys = {
            "unilwp.logging.verbose", 
            "unilwp.behavior.isolate",
            "unilwp.behavior.surface.updateRate",
            "unilwp.style.activity.launch.display",
            "unilwp.behavior.preview.button.settings",
            "unilwp.behavior.screen.off.early",
            "unilwp.behavior.activity.bypass.initial"
        };

        public string GetDescriptionForName()
        {
            switch (Name)
            {
                case "unilwp.logging.verbose":
                    return "Enable java logging of UniLWP internal within the aar scope";
                case "unilwp.behavior.isolate":
                    return "Only init UniLWP itself without actually init Unity";
                case "unilwp.behavior.activity.bypass.initial":
                    return "Certain Unity native plugins uses UnityPlayer.currentActivity, however it might be nullable with Live Wallpaper use cases. Enable this to spawn an empty activity as soon as the app is launched no matter from wallpaper services or activities";
                case "unilwp.version.android":
                    return "This is the version of the UniLWP Java lib, do not change it, it will be automatically included into the build";
                case "unilwp.version.unity":
                    return "This is the version of the UniLWP C# lib, do not change it, it will be automatically included into the build";
                case "unilwp.style.activity.launch.display":
                    return "This defines what should the launcher activity display\n0 = UnityFullscreen (Full-screen activity with Unity surface view)\n1 = UnityStable (Unity surface view clipped by navigation bar and status bar)\n2 = Preference (Work in progress, settings this has no effect)";
                case "unilwp.behavior.preview.button.settings":
                    return "This defines what should UniLWP trigger when the user clicks the settings icon in preview UI.\n0 = Start Launcher Activity\n1 = Notify Unity via C# only\n2 = Both";
            }
            return null;
        }
    }
}