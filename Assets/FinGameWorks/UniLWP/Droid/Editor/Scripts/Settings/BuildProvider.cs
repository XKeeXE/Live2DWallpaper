using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class BuildProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Build";
        public BuildProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new BuildProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.HelpBox("Learn different build types here", MessageType.Info);
                    
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Types", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("One-click"))
                    {
                        SettingsService.OpenProjectSettings(BuildSimpleProvider.Path);
                    }
                    EditorGUILayout.HelpBox("One-click option let you build live wallpaper apks just like you normally would with Unity games", MessageType.None);
                    EditorGUILayout.EndVertical();
                    
                    
                    EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Advanced Export"))
                    {
                        SettingsService.OpenProjectSettings(BuildAdvancedProvider.Path);
                    }
                    EditorGUILayout.HelpBox("Advanced export option generates a Android Studio project and you can develop your own features based on it", MessageType.None);
                    EditorGUILayout.EndVertical();
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            };
            return provider;
        }
    }
}