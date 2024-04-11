using System.Collections.Generic;
using System.Linq;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class BuildSimpleProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Build/One-Click";
        public BuildSimpleProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new BuildSimpleProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.HelpBox("Simple Build For Direct Apk/Aab Generation\nYou can also use Unity's default build menu (File/Build And Run) to trigger a simple build.", MessageType.Info);
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Actions", EditorStyles.centeredGreyMiniLabel);
                    
                    EditorGUILayout.BeginHorizontal();
                            
                    if (GUILayout.Button("Build (Debug)",GUILayout.ExpandWidth(true)))
                    {
                        Build(true);
                        GUIUtility.ExitGUI();
                    }
                            
                    if (GUILayout.Button("Build (Release)",GUILayout.ExpandWidth(true)))
                    {
                        Build(false);
                        GUIUtility.ExitGUI();
                    }  
                            
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.EndVertical();
                }
            };
            return provider;
        }
        
        public static void Build(bool debug)
        {
            string path = EditorUtility.SaveFilePanel("Save To", FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(""), "",
                "apk");
            if (!string.IsNullOrEmpty(path))
            {
                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
                    locationPathName = path,
                    target = BuildTarget.Android,
                    targetGroup = BuildTargetGroup.Android,
                    options = BuildOptions.CompressWithLz4
                };
                if (debug)
                {
                    buildPlayerOptions.options |= BuildOptions.EnableDeepProfilingSupport;
                    buildPlayerOptions.options |= BuildOptions.Development;
                }
                BuildPipeline.BuildPlayer(buildPlayerOptions);
            }
            
        }

    }
}