using System;
using System.Collections.Generic;
using System.Linq;
using FinGameWorks.UniLWP.Droid.Editor.Scripts.Helpers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class AdsProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Ads Integration";

        public AdsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes,
            keywords)
        {

        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new AdsProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    EditorGUILayout.HelpBox("Work in progress and API might change, use with caution", MessageType.Info);
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Settings", EditorStyles.centeredGreyMiniLabel);
                    if (!IsAdsEnabled())
                    {
                        EditorGUILayout.HelpBox("Activate Ads will\n1. add UNILWP_ADS flag to your symbol defines\n2. set unilwp.behavior.activity.bypass.initial to true in Behavior settings", MessageType.Warning);
                        if (GUILayout.Button("Activate"))
                        {
                            SetAdsFlag(true);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Deactivate Ads will remove UNILWP_ADS flag from your symbol defines", MessageType.Info);
                        if (GUILayout.Button("Deactivate"))
                        {
                            SetAdsFlag(false);
                        }
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();

                    if (IsAdsEnabled())
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Actions", EditorStyles.centeredGreyMiniLabel);
                        if (GUILayout.Button("Unity Ads Settings"))
                        {
                            try
                            {
                                SettingsService.OpenProjectSettings("Project/Services/Ads");
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                                SettingsService.OpenProjectSettings(Path);
                            }
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("User Guide", EditorStyles.centeredGreyMiniLabel);
                        if (GUILayout.Button("Online Guide"))
                        {
                            Application.OpenURL("https://unityads.unity3d.com/help/unity/integration-guide-unity");   
                        }
                        GUILayout.Label("1. Init Ads in Start()");
                        EditorGUILayout.SelectableLabel("Advertisement.Initialize (gameId, testMode);", EditorStyles.helpBox);
                        GUILayout.Label("2. Check if Ads module is loaded");
                        EditorGUILayout.SelectableLabel("Advertisement.IsReady()", EditorStyles.helpBox);
                        GUILayout.Label("3. Show Ads");
                        EditorGUILayout.SelectableLabel("Advertisement.Show();", EditorStyles.helpBox);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                    }
                }
            };
            return provider;
        }

        public static bool IsAdsEnabled()
        {
            return ProjectUtils.GetSymbolDefines().Contains("UNILWP_ADS") &&
                   BehaviorProvider.AndroidManifest != null &&
                   BehaviorProvider.AndroidManifest.Application.MetaDatas.FirstOrDefault(m =>
                       m.Name.Equals("unilwp.behavior.activity.bypass.initial") && m.Value.Equals("true")) != null
                ;
        }

        public static void SetAdsFlag(bool enable)
        {
            List<string> defines = ProjectUtils.GetSymbolDefines();
            if (enable && !defines.Contains("UNILWP_ADS"))
            {
                defines.Add("UNILWP_ADS");
            } else if (!enable && defines.Contains("UNILWP_ADS"))
            {
                defines.Remove("UNILWP_ADS");
            }
            string updatedStr = String.Join(";",defines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, updatedStr);

            if (BehaviorProvider.AndroidManifest != null && enable)
            {
                BehaviorProvider.AndroidManifest.Application.SetValueForMetaData("unilwp.behavior.activity.bypass.initial","true");
            }
        }
    }
}