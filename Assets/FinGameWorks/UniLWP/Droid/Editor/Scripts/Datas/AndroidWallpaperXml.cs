using System;
using System.Xml.Serialization;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas
{
    [XmlRoot("wallpaper")]
    public class AndroidWallpaperXml
    {
        [XmlNamespaceDeclarations] 
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public AndroidWallpaperXml()
        {
            xmlns.Add("android","http://schemas.android.com/apk/res/android");
        }
        
        /// <summary>
        /// API 29 constructor
        /// </summary>
        /// <param name="description"></param>
        /// <param name="showMetadataInPreview"></param>
        /// <param name="author"></param>
        /// <param name="contextUri"></param>
        /// <param name="contextDescription"></param>
        /// <param name="label"></param>
        /// <param name="thumbnail"></param>
        /// <param name="settingsActivity"></param>
        /// <param name="settingsSliceUri"></param>
        public AndroidWallpaperXml(string description, string showMetadataInPreview, string author, string contextUri, string contextDescription, string label, string thumbnail, string settingsActivity, string settingsSliceUri) : this(description, showMetadataInPreview, author, contextUri, contextDescription, label, thumbnail, settingsActivity)
        {
            SettingsSliceUri = settingsSliceUri;
        }

        /// <summary>
        /// API 25 constructor
        /// </summary>
        /// <param name="description"></param>
        /// <param name="showMetadataInPreview"></param>
        /// <param name="author"></param>
        /// <param name="contextUri"></param>
        /// <param name="contextDescription"></param>
        /// <param name="label"></param>
        /// <param name="thumbnail"></param>
        /// <param name="settingsActivity"></param>
        public AndroidWallpaperXml(string description, string showMetadataInPreview, string author, string contextUri, string contextDescription, string label, string thumbnail, string settingsActivity) : this(description, author, label, thumbnail, settingsActivity)
        {
            ShowMetadataInPreview = showMetadataInPreview;
            ContextUri = contextUri;
            ContextDescription = contextDescription;
        }

        /// <summary>
        /// API default constructor
        /// </summary>
        /// <param name="description"></param>
        /// <param name="author"></param>
        /// <param name="label"></param>
        /// <param name="thumbnail"></param>
        /// <param name="settingsActivity"></param>
        public AndroidWallpaperXml(string description, string author, string label, string thumbnail, string settingsActivity) : this()
        {
            Description = description;
            Author = author;
            Label = label;
            Thumbnail = thumbnail;
            SettingsActivity = settingsActivity;
        }

        [XmlAttribute(AttributeName = "description", Namespace = "http://schemas.android.com/apk/res/android")]
        public String Description;
        [XmlAttribute(AttributeName = "showMetadataInPreview", Namespace = "http://schemas.android.com/apk/res/android")]
        public String ShowMetadataInPreview;
        [XmlAttribute(AttributeName = "author", Namespace = "http://schemas.android.com/apk/res/android")]
        public String Author;
        [XmlAttribute(AttributeName = "contextUri", Namespace = "http://schemas.android.com/apk/res/android")]
        public String ContextUri;
        [XmlAttribute(AttributeName = "contextDescription", Namespace = "http://schemas.android.com/apk/res/android")]
        public String ContextDescription;
        [XmlAttribute(AttributeName = "label", Namespace = "http://schemas.android.com/apk/res/android")]
        public String Label = "@string/app_name";
        [XmlAttribute(AttributeName = "thumbnail", Namespace = "http://schemas.android.com/apk/res/android")]
        public String Thumbnail;
        [XmlAttribute(AttributeName = "settingsActivity", Namespace = "http://schemas.android.com/apk/res/android")]
        public String SettingsActivity = "com.justzht.unity.lwp.activity.LiveWallpaperSettingsRedirectActivity";
        [XmlAttribute(AttributeName = "settingsSliceUri", Namespace = "http://schemas.android.com/apk/res/android")]
        public String SettingsSliceUri;

        public AndroidWallpaperXml ToAPI29()
        {
            return new AndroidWallpaperXml(
                description: Description,
                showMetadataInPreview: ShowMetadataInPreview,
                author: Author,
                contextUri: ContextUri,
                contextDescription: ContextDescription,
                label: Label,
                thumbnail: Thumbnail,
                settingsActivity: SettingsActivity,
                settingsSliceUri: SettingsSliceUri
            );
        }
        public AndroidWallpaperXml ToAPI25()
        {
            return new AndroidWallpaperXml(
                description: Description,
                showMetadataInPreview: ShowMetadataInPreview,
                author: Author,
                contextUri: ContextUri,
                contextDescription: ContextDescription,
                label: Label,
                thumbnail: Thumbnail,
                settingsActivity: SettingsActivity
                );
        }
        
        public AndroidWallpaperXml ToAPIDefault()
        {
            return new AndroidWallpaperXml(
                description: Description,
                author: Author,
                label: Label,
                thumbnail: Thumbnail,
                settingsActivity: SettingsActivity
            );
        }
    }
}