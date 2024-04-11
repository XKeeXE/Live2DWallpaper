using System;
using System.Collections.Generic;
using System.IO;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
#if UNILWP_AUTHOR
    public class UtilsProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Utils";
        
        public UtilsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            
        }

        public static string BuildAarRelativePath = "Builds/Source/app/build/outputs/aar/UniLWP-debug.aar";
        public static String PathOfAarInSource() => FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildAarRelativePath);
        public static string PluginAarRelativePath = "Assets/FinGameWorks/UniLWP/Droid/Plugins/UniLWP-debug.aar";
        public static String PathOfAarInPlugin() => FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(PluginAarRelativePath);
        
        public static string JavaDocFolderRelativePath = "Builds/SourceDoc/java";
        public static string CSharpDocFolderRelativePath = "Builds/SourceDoc/csharp";

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new UtilsProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    if (GUILayout.Button("Update AAR"))
                    {
                        new FileInfo(PathOfAarInSource()).Replace(PathOfAarInPlugin(), null);
                        Debug.Log(PathOfAarInSource() + " Replaced " + PathOfAarInPlugin());
                    }
                    if (GUILayout.Button("Zip Java Source"))
                    {
                        
                    }
                    if (GUILayout.Button("Generate and Zip C# Doc"))
                    {
                        
                    }
                    if (GUILayout.Button("Generate and Zip Java Doc"))
                    {
                        
                    }
                }
            };
            return provider;
        }

    }
#endif
}