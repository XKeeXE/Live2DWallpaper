using System.Collections.Generic;
using System.IO;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class ResourcesProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Resources";

        public ResourcesProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new ResourcesProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("References", EditorStyles.centeredGreyMiniLabel);
                    
                    if (!PluginExist())
                    {
                        EditorGUILayout.HelpBox("Plugin not found at " + PluginPathRelative, MessageType.Error);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Editing resources directly within Unity Editor is a Unity 2020.1 and up only feature.\n(It actually would work in 2019.3+ if you put this plugin into Assets/Plugins/Android/ folder, but for clarity UniLWP.Droid would only maintain files within the Assets/FinGameWorks/UniLWP/Droid scope.)", MessageType.Info);
                    }

                    GUI.enabled = false;
                    EditorGUILayout.ObjectField(new GUIContent("Plugin"), AssetDatabase.LoadAssetAtPath(PluginPathRelative, typeof(Object)), typeof(Object), false);
                    GUI.enabled = true;
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Types", EditorStyles.centeredGreyMiniLabel);
                    if (GUILayout.Button("Strings.xml"))
                    {
                        SettingsService.OpenProjectSettings(ResourcesStringProvider.Path);
                    }
                    if (GUILayout.Button("Wallpaper.xml"))
                    {
                        SettingsService.OpenProjectSettings(ResourcesWallpaperProvider.Path);
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            };
            return provider;
        }
        
        
        public static readonly string PluginPathRelative = "Assets/FinGameWorks/UniLWP/Droid/Plugins/unilwp.customize.androidlib";
        public static string GetPluginPath()
        {
            return FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(PluginPathRelative);
        }
        public static bool PluginExist()
        {
            return Directory.Exists(GetPluginPath());
        }
    }
}