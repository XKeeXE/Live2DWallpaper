using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Datas;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class BuildAdvancedProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Build/Advanced Export";
        public BuildAdvancedProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new BuildAdvancedProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    BuildPathInfo pathInfo = BuildPathInfo.GetInstanceInEditor();
                    if (pathInfo != null)
                    {
                        EditorGUILayout.HelpBox("Advanced Export For Further Modification In Android Studio", MessageType.Info);
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Path", EditorStyles.centeredGreyMiniLabel);

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Main Project", EditorStyles.centeredGreyMiniLabel);
                        EditorGUILayout.BeginHorizontal();
                        pathInfo.buildOutPathRelativeToProj = EditorGUILayout.TextField("Relative Path" ,pathInfo.buildOutPathRelativeToProj);
                        if (GUILayout.Button("Open"))
                        {
                            EditorUtility.RevealInFinder(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj));
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Absolute Path");
                        EditorGUILayout.SelectableLabel(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj), EditorStyles.helpBox);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Temp Project (Debug)", EditorStyles.centeredGreyMiniLabel);
                        EditorGUILayout.BeginHorizontal();
                        pathInfo.buildTempDebugPathRelativeToProj = EditorGUILayout.TextField("Relative Path" ,pathInfo.buildTempDebugPathRelativeToProj);
                        if (GUILayout.Button("Open"))
                        {
                            EditorUtility.RevealInFinder(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildTempDebugPathRelativeToProj));
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Absolute Path");
                        EditorGUILayout.SelectableLabel(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildTempDebugPathRelativeToProj), EditorStyles.helpBox);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Temp Project (Release)", EditorStyles.centeredGreyMiniLabel);
                        EditorGUILayout.BeginHorizontal();
                        pathInfo.buildTempReleasePathRelativeToProj = EditorGUILayout.TextField("Relative Path" ,pathInfo.buildTempReleasePathRelativeToProj);
                        if (GUILayout.Button("Open"))
                        {
                            EditorUtility.RevealInFinder(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildTempReleasePathRelativeToProj));
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Absolute Path");
                        EditorGUILayout.SelectableLabel(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildTempReleasePathRelativeToProj), EditorStyles.helpBox);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.Space();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Actions", EditorStyles.centeredGreyMiniLabel);
                        if (pathInfo.BuildOutPathExist)
                        {
                            EditorGUILayout.HelpBox("You have a main project properly configured. To update the content of your main project, trigger a build of either debug or release variant, and they will be copied into the main project.\nIf your Android Studio loses project structure after a build, just click the button 'sync project with gradle files' at the top-right of Android Studio.", MessageType.Info);
                            EditorGUILayout.BeginHorizontal();
                            
                            EditorGUILayout.BeginVertical();
                            if (GUILayout.Button("Export (Debug)",GUILayout.ExpandWidth(true)))
                            {
                                BuildDebug();
                                GUIUtility.ExitGUI();
                            }
                            EditorGUILayout.HelpBox("Command/Ctrl + Shift + D", MessageType.None);
                            EditorGUILayout.EndVertical();
                            
                            EditorGUILayout.BeginVertical();
                            if (GUILayout.Button("Export (Release)",GUILayout.ExpandWidth(true)))
                            {
                                BuildRelease();
                                GUIUtility.ExitGUI();
                            }  
                            EditorGUILayout.HelpBox("Command/Ctrl + Shift + R", MessageType.None);
                            EditorGUILayout.EndVertical();
                            
                            EditorGUILayout.EndHorizontal();
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("You do not have a main project properly configured. If this is your first using UniLWP.Droid advanced export, please first init your main project. The main project, as long as you are editing the launcher module via Android Studio (in contrast to the unityLibrary module which would be replaced every time you export from Unity Editor), would maintain all your changes.", MessageType.Warning);
                            if (GUILayout.Button("Init Project"))
                            {
                                InitProject();
                                GUIUtility.ExitGUI();
                            }  
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Misc", EditorStyles.centeredGreyMiniLabel);
                        if (pathInfo.BuildOutPathExist)
                        {
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("Fix Library Gradle File", GUILayout.Width(100),GUILayout.ExpandWidth(true)))
                            {
                                ModifyLibraryGradleFile();
                            }
                            if (GUILayout.Button("Fix Library Manifest File", GUILayout.Width(100),GUILayout.ExpandWidth(true)))
                            {
                                ModifyLibraryManifestFile();
                            }
                            if (GUILayout.Button("Delete Modules.xml",GUILayout.Width(100),GUILayout.ExpandWidth(true)))
                            {
                                DeleteModulesXML();
                            }  
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("BuildPathInfo Not Found", MessageType.Error);
                    }
                }
            };
            return provider;
        }

        [MenuItem("FinGameWorks/UniLWP/Droid/Build And Copy/Release _#%r", true)]
        static bool CanCallBuildRelease()
        {
            BuildPathInfo pathInfo = BuildPathInfo.GetInstanceInEditor();
            return pathInfo != null && pathInfo.BuildOutPathExist;
        }
        
        [MenuItem("FinGameWorks/UniLWP/Droid/Build And Copy/Release _#%r")]
        public static void BuildRelease()
        {
            Build(false);
        }
        
        [MenuItem("FinGameWorks/UniLWP/Droid/Build And Copy/Debug _#%d", true)]
        static bool CanCallBuildDebug()
        {
            BuildPathInfo pathInfo = BuildPathInfo.GetInstanceInEditor();
            return pathInfo != null && pathInfo.BuildOutPathExist;
        }
        
        [MenuItem("FinGameWorks/UniLWP/Droid/Build And Copy/Debug _#%d")]
        public static void BuildDebug()
        {
            Build(true);
        }

        public static void Build(bool debug)
        {
            string path = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(
                debug
                    ? BuildPathInfo.GetInstanceInEditor().buildTempDebugPathRelativeToProj
                    : BuildPathInfo.GetInstanceInEditor().buildTempReleasePathRelativeToProj
            );
            Debug.Log("Temp Target Path is: " + path);
            if (Directory.Exists(path)) { Directory.Delete(path,true); }
            FileUtils.CreateDirectoryIfNotExist(path);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
                locationPathName = path,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.CompressWithLz4 |
                          BuildOptions.AcceptExternalModificationsToPlayer
            };
            if (debug)
            {
                buildPlayerOptions.options |= BuildOptions.EnableDeepProfilingSupport;
                buildPlayerOptions.options |= BuildOptions.Development;
            }
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
            BuildPipeline.BuildPlayer(buildPlayerOptions);   
        }
        
        [MenuItem("FinGameWorks/UniLWP/Droid/Misc/Init Main Project", true)]
        static bool CanCallInitMainProject()
        {
            BuildPathInfo pathInfo = BuildPathInfo.GetInstanceInEditor();
            return pathInfo != null && !pathInfo.BuildOutPathExist;
        }

        [MenuItem("FinGameWorks/UniLWP/Droid/Misc/Init Main Project")]
        public static void InitProject()
        {
            string path = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj);
            if (EditorUtility.DisplayDialog("Sure?","Will build to " + path,"OK","Cancel"))
            {
                FileUtils.CreateDirectoryIfNotExist(path);
                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
                    locationPathName = path,
                    target = BuildTarget.Android,
                    targetGroup = BuildTargetGroup.Android,
                    options = BuildOptions.CompressWithLz4 |
                              BuildOptions.AcceptExternalModificationsToPlayer
                };
                EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
                BuildPipeline.BuildPlayer(buildPlayerOptions);   
            }
        }

        [MenuItem("FinGameWorks/UniLWP/Droid/Copy Only/Release")]
        public static void CopyRelease()
        {
            FileUtils.CopyUnityLibraryFromToMain(
                FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor()
                    .buildTempReleasePathRelativeToProj));
            ModifyLibraryGradleFile();
            ModifyLibraryManifestFile();
            DeleteModulesXML();
        }

        [MenuItem("FinGameWorks/UniLWP/Droid/Copy Only/Debug")]
        public static void CopyDebug()
        {
            FileUtils.CopyUnityLibraryFromToMain(
                FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor()
                    .buildTempDebugPathRelativeToProj));
            ModifyLibraryGradleFile();
            ModifyLibraryManifestFile();
            DeleteModulesXML();
        }

        [MenuItem("FinGameWorks/UniLWP/Droid/Misc/Delete Module.xml")]
        public static void DeleteModulesXML()
        {
            string projectRoot = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj);
            string filePath = System.IO.Path.Combine(projectRoot, ".idea", "modules.xml");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [MenuItem("FinGameWorks/UniLWP/Droid/Misc/Fix Generated Library Gradle File")]
        public static void ModifyLibraryGradleFile()
        {
            string gradlePath = System.IO.Path.Combine(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj), "unityLibrary", "build.gradle");
            File.WriteAllText(gradlePath, File.ReadAllText(gradlePath).Replace("implementation(name: 'UniLWP-debug', ext:'aar')","api(name: 'UniLWP-debug', ext:'aar')"));
            File.WriteAllText(gradlePath, File.ReadAllText(gradlePath).Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])","api fileTree(dir: 'libs', include: ['*.jar'])"));
        }
        
        [MenuItem("FinGameWorks/UniLWP/Droid/Misc/Fix Generated Library Manifest File")]
        public static void ModifyLibraryManifestFile()
        {
            string manifestPath = System.IO.Path.Combine(FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo.GetInstanceInEditor().buildOutPathRelativeToProj), "unityLibrary", "src", "main", "AndroidManifest.xml");
            string manifest = File.ReadAllText(manifestPath);
            manifest = Regex.Replace(manifest, @"<activity .*android:name=""com\.unity3d\.player\.UnityPlayerActivity""[\s\S]*?</activity>", "");
            File.WriteAllText(manifestPath, manifest);
        }

        [PostProcessBuild(Int32.MaxValue)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
#if UNITY_EDITOR_WIN
            pathToBuiltProject = pathToBuiltProject.ToUnixPathStyle();
#endif
            string release = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo
                .GetInstanceInEditor()
                .buildTempReleasePathRelativeToProj);
            string debug = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(BuildPathInfo
                .GetInstanceInEditor()
                .buildTempDebugPathRelativeToProj);
            if (pathToBuiltProject.Equals(release))
            {
                // do something here to remove com.unity3d.player.UnityPlayerActivity
                CopyRelease();
                Debug.Log("OnPostprocessBuild Release");
            } else if (pathToBuiltProject.Equals(debug))
            {
                // do something here to remove com.unity3d.player.UnityPlayerActivity
                CopyDebug();
                Debug.Log("OnPostprocessBuild Debug");
            }
            else
            {
                Debug.LogWarning("OnPostprocessBuild No Ops");
            }
        }
    }
}