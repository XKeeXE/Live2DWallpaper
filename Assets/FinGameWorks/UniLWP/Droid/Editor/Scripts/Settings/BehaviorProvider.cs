using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class BehaviorProvider : SettingsProvider
    {
        public static AndroidManifestXml AndroidManifest = new AndroidManifestXml();
        public static string Path = "UniLWP/Droid/Behavior";
        
        public BehaviorProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            LoadXML();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new BehaviorProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    if (ManifestExist())
                    {
                        EditorGUILayout.HelpBox("Setting live wallpaper behaviour here. Any modification will be updated to the AndroidManifest xml file", MessageType.Info);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Absolute Path");
                        EditorGUILayout.SelectableLabel(GetManifestPath(), EditorStyles.helpBox);
                        EditorGUILayout.EndHorizontal();
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
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Meta-data Entries", EditorStyles.boldLabel);
                        if (GUILayout.Button("+ Add"))
                        {
                            void MenuItemClicked(object key)
                            {
                                AndroidManifest.Application.MetaDatas.Add(new AndroidManifestMetaData((string) key, ""));
                            }
                            GenericMenu menu = new GenericMenu();
                            menu.AddItem(new GUIContent("custom..."), false, MenuItemClicked, "");
                            foreach (string key in AndroidManifestMetaData.AvailableMetadataKeys.Except(
                                AndroidManifest.Application.MetaDatas.Select(metaData => metaData.Name)))
                            {
                                menu.AddItem(new GUIContent(key), false, MenuItemClicked, key);
                            }
                            menu.ShowAsContext();
                        }
                        EditorGUILayout.EndHorizontal();

                        HashSet<AndroidManifestMetaData> toRemoveMetaDatas = new HashSet<AndroidManifestMetaData>();
                        foreach (AndroidManifestMetaData metaData in AndroidManifest.Application.MetaDatas)
                        {
                            EditorGUILayout.BeginHorizontal("box");
                            GUILayout.Label("=", GUILayout.Width(10));
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.BeginHorizontal();
                            metaData.Name = EditorGUILayout.TextField(metaData.Name);
                            metaData.Value = EditorGUILayout.TextField(metaData.Value);
                            EditorGUILayout.EndHorizontal();
                            string description = metaData.GetDescriptionForName();
                            if (description != null)
                            {
                                EditorGUILayout.HelpBox(description, MessageType.None);
                            }
                            EditorGUILayout.EndVertical();
                            if (GUILayout.Button("-",GUILayout.Width(30)))
                            {
                                toRemoveMetaDatas.Add(metaData);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        foreach (AndroidManifestMetaData metaData in toRemoveMetaDatas)
                        {
                            AndroidManifest.Application.MetaDatas.Remove(metaData);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("AndroidManifest.xml not found", MessageType.Error);
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

        public static readonly string ManifestPathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/AndroidManifest.xml";
        public static string GetManifestPath()
        {
            return FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(ManifestPathRelative);
        }

        public static Boolean ManifestExist()
        {
            return File.Exists(GetManifestPath());
        }

        public static void ResetXML()
        {
            AndroidManifest = new AndroidManifestXml();
        }

        public static void LoadXML()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AndroidManifestXml));
                FileStream fs = new FileStream(GetManifestPath(), FileMode.Open);
                AndroidManifest = (AndroidManifestXml) xmlSerializer.Deserialize(fs);
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
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AndroidManifestXml));
                TextWriter wr = new StreamWriter(GetManifestPath());
                xmlSerializer.Serialize(wr, AndroidManifest);
                wr.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}