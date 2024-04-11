using System.Collections.Generic;
using System.IO;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class MainProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid";
        public MainProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            
        }
        
        public static string JavaSourceRelativePath = "Assets/FinGameWorks/UniLWP/Droid/References/JavaSource.zip";
        public static string PdfTutorialRelativePath = "Assets/FinGameWorks/UniLWP/Droid/References/UniLWP.Droid Documentation.pdf";
        public static string JavaDocZipRelativePath = "Assets/FinGameWorks/UniLWP/Droid/References/JavaDoc.zip";
        public static string CSharpDocZipRelativePath = "Assets/FinGameWorks/UniLWP/Droid/References/CSharpDoc.zip";
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new MainProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.HelpBox("Welcome to UniLWP.Droid", MessageType.Info);
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Info", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.LabelField("Version", Droid.Scripts.UniLWP.FullVersion(),EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Release Date", Droid.Scripts.UniLWP.ReleaseDate.ToShortDateString(),EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Documentation", EditorStyles.centeredGreyMiniLabel);
                    if (GUILayout.Button("Online Tutorial"))
                    {
                        Application.OpenURL("http://unilwpdroid.rtfd.io/");
                    }

                    string pdfTutorialAbsolutePath = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(PdfTutorialRelativePath);
                    GUI.enabled = File.Exists(pdfTutorialAbsolutePath);
                    if (GUILayout.Button("PDF Tutorial"))
                    {
                        EditorUtility.RevealInFinder(pdfTutorialAbsolutePath);
                    }
                    GUI.enabled = true;
                    GUI.enabled = false;
                    if (GUILayout.Button("Sample (WIP)"))
                    {
                    }
                    GUI.enabled = true;
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Reference", EditorStyles.centeredGreyMiniLabel);
                    EditorGUILayout.HelpBox("Do not unzip those files within the Unity project, or Unity will import uncompressed html files as Unity text assets", MessageType.Warning);
                    string javaSourceAbsolutePath = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(JavaSourceRelativePath);
                    GUI.enabled = File.Exists(javaSourceAbsolutePath);
                    if (GUILayout.Button("Source Code (Java)"))
                    {
                        EditorUtility.RevealInFinder(javaSourceAbsolutePath);
                    }
                    GUI.enabled = true;
                    string javaDocAbsolutePath = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(JavaDocZipRelativePath);
                    GUI.enabled = File.Exists(javaDocAbsolutePath);
                    if (GUILayout.Button("Code Documentation (Java)"))
                    {
                        EditorUtility.RevealInFinder(javaDocAbsolutePath);
                    }
                    GUI.enabled = true;
                    string csharpDocAbsolutePath = FileUtils.GetAbsolutePathFromRelativePathToProjectRoot(CSharpDocZipRelativePath);
                    GUI.enabled = File.Exists(csharpDocAbsolutePath);
                    if (GUILayout.Button("Code Documentation (C#)"))
                    {
                        EditorUtility.RevealInFinder(csharpDocAbsolutePath);
                    }
                    GUI.enabled = true;
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Contact", EditorStyles.centeredGreyMiniLabel);
                    if (GUILayout.Button("Email"))
                    {
                        Application.OpenURL("mailto:justzht+unilwpdroid@gmail.com");
                    }
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Asset Store (Paid Ver.)",GUILayout.Width(100),GUILayout.ExpandWidth(true)))
                    {
                        Application.OpenURL("http://u3d.as/1QVw");
                    }
                    if (GUILayout.Button("GitHub (Free Ver.)",GUILayout.Width(100), GUILayout.ExpandWidth(true)))
                    {
                        Application.OpenURL("https://github.com/JustinFincher/UniLWP.Droid.Package.Free");
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                },
                titleBarGuiHandler = () =>
                {
                    
                }
            };
            return provider;
        }
    }
}