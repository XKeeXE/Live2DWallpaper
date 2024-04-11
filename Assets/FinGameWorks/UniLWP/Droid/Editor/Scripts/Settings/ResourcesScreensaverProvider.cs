using System.Collections.Generic;
using UnityEditor;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class ResourcesScreensaverProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Resources/Screen Saver";

        public ResourcesScreensaverProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new ResourcesScreensaverProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.HelpBox("Work in progress", MessageType.Info);
                }
            };
            return provider;
        }
    }
}