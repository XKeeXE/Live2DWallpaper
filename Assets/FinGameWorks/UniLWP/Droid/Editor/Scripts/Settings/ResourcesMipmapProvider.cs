using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class ResourcesMipmapProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Resources/Mipmap";
        
        public ResourcesMipmapProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(
            path, scopes, keywords)
        {
            Refresh();
        }


        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new ResourcesProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.HelpBox("All files will be merged into the final app", MessageType.Info);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Actions", EditorStyles.centeredGreyMiniLabel);
                    if (GUILayout.Button("Refresh"))
                    {
                        Refresh();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Files", EditorStyles.boldLabel);
                    if (GUILayout.Button("+ Add"))
                    {
                        string imageFilePath = EditorUtility.OpenFilePanelWithFilters("Choose Image To Import", Application.dataPath, new string[] { "Image files", "png,jpg,jpeg" });
                        if (!String.IsNullOrEmpty(imageFilePath))
                        {
                            FileInfo imageFileInfo = new FileInfo(imageFilePath);
                            if (imageFileInfo.Exists)
                            {
                                string copyToFileName = System.IO.Path.Combine(
                                    FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(MipmapFolderPathRelative),
                                    imageFileInfo.Name
                                );
                                Debug.Log("Copy " + imageFilePath + " to " + copyToFileName);
                                File.Copy(imageFilePath, copyToFileName);
                                Refresh();
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    HashSet<string> toRemoveImages = new HashSet<string>();
                    foreach (FileInfo mipmap in Mipmaps)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        GUILayout.Label("=", GUILayout.Width(10));
                        GUI.enabled = false;
                        EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath(
                                FileUtils.GetRelativePathFromAbsolutePathToProjectRoot(mipmap.FullName),
                                typeof(DefaultAsset)),
                            typeof(DefaultAsset), false);
                        GUI.enabled = true;
                        if (GUILayout.Button("-",GUILayout.Width(30)))
                        {
                            toRemoveImages.Add(mipmap.FullName);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    foreach (string mipmap in toRemoveImages)
                    {
                        File.Delete(mipmap);
                    }
                    if (toRemoveImages.Count > 0)
                    {
                        Refresh();
                    }
                }
            };
            return provider;
        }
        
        public static readonly string MipmapFolderPathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib/res/mipmap";

        public static List<FileInfo> Mipmaps = new List<FileInfo>();
        public static void Refresh()
        {
            Mipmaps = Directory.GetFiles(
                    FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(MipmapFolderPathRelative))
                .Select(s => new FileInfo(s))
                .Where(f => f.Extension.Equals(".jpg") || f.Extension.Equals(".png") || f.Extension.Equals(".jpeg"))
                .ToList();
            AssetDatabase.Refresh();
        }
    }
}