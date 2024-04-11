using System;
using System.IO;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers
{
    public class FileUtils
    {
        public static void CreateDirectoryIfNotExist(string fullPath)
        {
            Directory.CreateDirectory(fullPath);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static string GetAbsolutePathFromRelativePathToProjectRoot(string path)
        {
#if UNITY_EDITOR_WIN
            path = path.ToUnixPathStyle();
#endif
            DirectoryInfo assetInfo = new DirectoryInfo(Application.dataPath);
            Debug.Assert(assetInfo.Parent != null, "assetInfo.Parent != null");
            return Path.Combine(assetInfo.Parent.FullName, path);
        }
        
        public static string GetRelativePathFromAbsolutePathToProjectRoot(string path)
        {
#if UNITY_EDITOR_WIN
            path = path.ToUnixPathStyle();
#endif
            DirectoryInfo assetInfo = new DirectoryInfo(Application.dataPath);
            Debug.Assert(assetInfo.Parent != null, "assetInfo.Parent != null");
            return path.Substring(assetInfo.Parent.FullName.Length + 1);
        }

        public static void CopyUnityLibraryFromToMain(String originalBuiltPath)
        {
            string targetBuiltPath = GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj);
            DirectoryInfo fromUnityLibrary = new DirectoryInfo(Path.Combine(originalBuiltPath,"unityLibrary"));
            DirectoryInfo toUnityLibrary = new DirectoryInfo(Path.Combine(targetBuiltPath,"unityLibrary"));
            FileUtil.DeleteFileOrDirectory(toUnityLibrary.FullName);
            CopyAll(fromUnityLibrary,toUnityLibrary);
            
            FileInfo fromSettingsGradle = new FileInfo(Path.Combine(originalBuiltPath,"settings.gradle"));
            FileInfo toSettingsGradle = new FileInfo(Path.Combine(targetBuiltPath,"settings.gradle"));
            fromSettingsGradle.CopyTo(toSettingsGradle.FullName, true);
        }
    }
}