using System;
using System.Collections.Generic;
using FinGameWorks.UniLWP.Droid.Scripts.Datas;
using FinGameWorks.UniLWP.Droid.Scripts.Managers;
using UnityEditor;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Editor.Scripts.Settings
{
    public class SimulatorProvider : SettingsProvider
    {
        public static string Path = "UniLWP/Droid/Simulator";
        public SimulatorProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

#if UNITY_ANDROID
        public static bool SimulatorInDarkMode
        {
            get => _simulatorInDarkMode;
            set
            {
                _simulatorInDarkMode = value;
                LiveWallpaperManagerDroid.Instance?.darkModeEnableUpdated?.Invoke(value);
            }
        }
        private static bool _simulatorInDarkMode;

        public static Enums.ScreenStatus SimulatorScreenStatus
        {
            get => _simulatorScreenStatus;
            set
            {
                _simulatorScreenStatus = value;
                LiveWallpaperManagerDroid.Instance?.screenDisplayStatusUpdated?.Invoke(value);
            }
        }
        private static Enums.ScreenStatus _simulatorScreenStatus;
#endif

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SimulatorProvider(Path, SettingsScope.Project)
            {
                guiHandler = (search) =>
                {
                    if (Application.isPlaying)
                    {
                        
#if UNITY_ANDROID
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Lock Events", EditorStyles.centeredGreyMiniLabel);
                        SimulatorScreenStatus = (Enums.ScreenStatus) EditorGUILayout.EnumPopup("State", SimulatorScreenStatus);
                        EditorGUILayout.BeginHorizontal();
                        switch (SimulatorScreenStatus)
                        {
                            case Enums.ScreenStatus.LockedAndOff:
                            case Enums.ScreenStatus.LockedAndAOD:
                                if (GUILayout.Button("Light-up Screen (Power Button)"))
                                {
                                    SimulatorScreenStatus = Enums.ScreenStatus.LockedAndOn;
                                }
                                if (GUILayout.Button("Unlock (Fingerprint)"))
                                {
                                    SimulatorScreenStatus = Enums.ScreenStatus.LockedAndOn;
                                    SimulatorScreenStatus = Enums.ScreenStatus.Unlocked;
                                }
                                break;
                            case Enums.ScreenStatus.LockedAndOn:
                                if (GUILayout.Button("CLose Screen (Power Button)"))
                                {
                                    SimulatorScreenStatus = Enums.ScreenStatus.LockedAndOff;
                                }
                                if (GUILayout.Button("Unlock (Fingerprint)"))
                                {
                                    SimulatorScreenStatus = Enums.ScreenStatus.Unlocked;
                                }
                                break;
                            case Enums.ScreenStatus.Unlocked:
                                if (GUILayout.Button("Lock (Power Button)"))
                                {
                                    SimulatorScreenStatus = Enums.ScreenStatus.LockedAndOff;
                                }
                                break;
                            default:
                                break;
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Launcher", EditorStyles.centeredGreyMiniLabel);
                        EditorGUILayout.Slider("Scroll", 0, 0, 1);
                        EditorGUILayout.EndVertical();
                        
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Environments", EditorStyles.centeredGreyMiniLabel);
                        SimulatorInDarkMode = GUILayout.Toggle(SimulatorInDarkMode, "Dark Mode");
                        EditorGUILayout.EndVertical();
                        
#endif
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Simulator only works in play mode", MessageType.Warning);
                        if (GUILayout.Button("Play"))
                        {
                            EditorApplication.EnterPlaymode();
                        }
                    }
                }
            };
            return provider;
        }
    }
}