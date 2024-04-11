using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class ResourcesWallpaperProvider : SettingsProvider
    {
        public static AndroidWallpaperXml AndroidWallpaper = new AndroidWallpaperXml();
        public static string Path = "UniLWP/Droid/Resources/Wallpaper";
        public ResourcesWallpaperProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            LoadXML();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new ResourcesWallpaperProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    if (WallpaperXmlExist())
                    {
                        EditorGUILayout.HelpBox("Edit info about your wallpaper.\nAll fields would be serialized to unilwp_wallpaper.xml and shown on Android wallpaper preview UI.", MessageType.Info);

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Actions", EditorStyles.centeredGreyMiniLabel);
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Load XML"))
                        {
                            LoadXML();
                        }
                        if (GUILayout.Button("Save XML"))
                        {
                            SaveXML();
                        }  
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Default", EditorStyles.centeredGreyMiniLabel);
                        AndroidWallpaper.Label = EditorGUILayout.TextField("Label", AndroidWallpaper.Label);
                        AndroidWallpaper.Author = EditorGUILayout.TextField("Author", AndroidWallpaper.Author);
                        AndroidWallpaper.Description = EditorGUILayout.TextField("Description", AndroidWallpaper.Description);
                        AndroidWallpaper.SettingsActivity = EditorGUILayout.TextField("SettingsActivity", AndroidWallpaper.SettingsActivity);
                        AndroidWallpaper.Thumbnail = EditorGUILayout.TextField("Thumbnail", AndroidWallpaper.Thumbnail);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Available on API 25 (Android 7.1)", EditorStyles.centeredGreyMiniLabel);
                        AndroidWallpaper.ContextUri = EditorGUILayout.TextField("ContextUri", AndroidWallpaper.ContextUri);
                        AndroidWallpaper.ContextDescription = EditorGUILayout.TextField("ContextDescription", AndroidWallpaper.ContextDescription);
                        AndroidWallpaper.ShowMetadataInPreview = EditorGUILayout.TextField("ShowMetadataInPreview", AndroidWallpaper.ShowMetadataInPreview);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Available on API 29 (Android 10)", EditorStyles.centeredGreyMiniLabel);
                        AndroidWallpaper.SettingsSliceUri = EditorGUILayout.TextField("SettingsSliceUri", AndroidWallpaper.SettingsSliceUri);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("unilwp_wallpaper.xml not found", MessageType.Error);
                    }
                },
                titleBarGuiHandler = () =>
                {
                    if (GUILayout.Button("Reset"))
                    {
                        ResetXML();
                    }
                }
            };
            return provider;
        }
        
        public static readonly string WallpaperXmlTemplatePathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/res/xml/unilwp_wallpaper_template.xml";
        public static readonly string WallpaperXmlDefaultPathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/res/xml/unilwp_wallpaper.xml";
        public static readonly string WallpaperXmlV25PathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/res/xml-v25/unilwp_wallpaper.xml";
        public static readonly string WallpaperXmlV29PathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/res/xml-v29/unilwp_wallpaper.xml";
        
        public static string GetWallpaperXmlPath()
        {
            return FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(WallpaperXmlTemplatePathRelative);
        }

        public static Boolean WallpaperXmlExist()
        {
            return File.Exists(GetWallpaperXmlPath());
        }

        public static void ResetXML()
        {
            AndroidWallpaper = new AndroidWallpaperXml();
        }

        public static void LoadXML()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AndroidWallpaperXml));
                FileStream fs = new FileStream(GetWallpaperXmlPath(), FileMode.Open);
                AndroidWallpaper = (AndroidWallpaperXml) xmlSerializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void SaveXML()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AndroidWallpaperXml));
                using (TextWriter wr = new StreamWriter(GetWallpaperXmlPath()))
                {
                    xmlSerializer.Serialize(wr, AndroidWallpaper);
                }
                using (TextWriter wr = new StreamWriter(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(WallpaperXmlDefaultPathRelative)))
                {
                    xmlSerializer.Serialize(wr, AndroidWallpaper.ToAPIDefault());
                }
                using (TextWriter wr = new StreamWriter(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(WallpaperXmlV25PathRelative)))
                {
                    xmlSerializer.Serialize(wr, AndroidWallpaper.ToAPI25());
                }
                using (TextWriter wr = new StreamWriter(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(WallpaperXmlV29PathRelative)))
                {
                    xmlSerializer.Serialize(wr, AndroidWallpaper.ToAPI29());
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    
}