using System.IO;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas
{
    [CreateAssetMenu(menuName = "FinGameWorks/UniLWP/Droid/BuildPathInfo",fileName = "BuildPathInfo")]
    public class BuildPathInfo : ScriptableObject
    {
        public static readonly string DefaultObjectPath = "Assets/FinGameWorks/UniLWP/Droid/Editor/Settings/BuildPathInfo.asset";
        public string buildTempReleasePathRelativeToProj = "Builds/Android/Temp-Release";
        public bool BuildTempReleasePathExist => Directory.Exists(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(buildTempReleasePathRelativeToProj));

        public string buildTempDebugPathRelativeToProj = "Builds/Android/Temp-Debug";
        public bool BuildTempDebugPathExist => Directory.Exists(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(buildTempDebugPathRelativeToProj));
        public string buildOutPathRelativeToProj = "Builds/Android/Main";
        public bool BuildOutPathExist => Directory.Exists(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(buildOutPathRelativeToProj));

#if UNITY_EDITOR
        public static BuildPathInfo GetInstanceInEditor()
        {
            if (!File.Exists(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(DefaultObjectPath)))
            {
                BuildPathInfo asset = ScriptableObject.CreateInstance<BuildPathInfo>();
                AssetDatabase.CreateAsset(asset, DefaultObjectPath);
                AssetDatabase.SaveAssets();
            }
            return AssetDatabase.LoadAssetAtPath<BuildPathInfo>(BuildPathInfo.DefaultObjectPath);
        }

#endif
    }
}