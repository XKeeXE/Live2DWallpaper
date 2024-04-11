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
    public class ResourcesStringProvider : SettingsProvider
    {
        public static AndroidResourceStringXml AndroidResourceString = new AndroidResourceStringXml();
        public static string Path = "UniLWP/Droid/Resources/Strings";

        public ResourcesStringProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(
            path, scopes, keywords)
        {
            LoadXML();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new ResourcesStringProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    if (StringsXmlExist())
                    {
                        EditorGUILayout.HelpBox("All fields would be serialized to strings.xml which you can reference in other xml files or retrieve in runtime.", MessageType.Info);

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
                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("String Entries", EditorStyles.boldLabel);
                        if (GUILayout.Button("+ Add"))
                        {
                            AndroidResourceString.Strings.Add(new AndroidResourceStringEntry("", ""));
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        HashSet<AndroidResourceStringEntry> toRemoveMetaDatas = new HashSet<AndroidResourceStringEntry>();
                        foreach (AndroidResourceStringEntry entry in AndroidResourceString.Strings)
                        {
                            EditorGUILayout.BeginHorizontal("box");
                            GUILayout.Label("=", GUILayout.Width(10));
                            entry.Name = EditorGUILayout.TextField(entry.Name);
                            entry.Value = EditorGUILayout.TextField(entry.Value);
                            if (GUILayout.Button("-",GUILayout.Width(30)))
                            {
                                toRemoveMetaDatas.Add(entry);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        foreach (AndroidResourceStringEntry entry in toRemoveMetaDatas)
                        {
                            AndroidResourceString.Strings.Remove(entry);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("strings.xml not found", MessageType.Error);
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
        
        public static readonly string StringsXmlPathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/res/values/strings.xml";
        public static string GetStringsXmlPath()
        {
            return FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(StringsXmlPathRelative);
        }

        public static Boolean StringsXmlExist()
        {
            return File.Exists(GetStringsXmlPath());
        }

        public static void ResetXML()
        {
            AndroidResourceString = new AndroidResourceStringXml();
        }

        public static void LoadXML()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AndroidResourceStringXml));
                FileStream fs = new FileStream(GetStringsXmlPath(), FileMode.Open);
                AndroidResourceString = (AndroidResourceStringXml) xmlSerializer.Deserialize(fs);
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
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AndroidResourceStringXml));
                TextWriter wr = new StreamWriter(GetStringsXmlPath());
                xmlSerializer.Serialize(wr, AndroidResourceString);
                wr.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}