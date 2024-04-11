using System;
using System.IO;
using System.Linq;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Windows
{
    public class MenuActions : EditorWindow
    {

        [MenuItem("FinGameWorks/UniLWP/Droid/Settings")]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings(MainProvider.Path);
        }
    }
}