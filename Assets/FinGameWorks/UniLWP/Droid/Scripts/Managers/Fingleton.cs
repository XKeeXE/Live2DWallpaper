using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Scripts.Managers
{
    
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(FingletonBase),true)]
    public class FingletonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            FingletonBase targetObj = (FingletonBase)target;
            UnityEditor.EditorGUILayout.LabelField("Type",targetObj.GetType().FullName);
            base.OnInspectorGUI();
        }
    }
#endif
    
    /// <summary>
    /// ScriptableObject Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class Fingleton<T> : FingletonBase where T : FingletonBase {
        public static T Instance { get { return Initializer<T>.LazyInstance; } }

        private static T _instance;
        protected override void CreateInstance() { }
        
        // ReSharper disable once ClassNeverInstantiated.Local
#pragma warning disable CS0693
        private class Initializer<T> where T : FingletonBase {
#pragma warning restore CS0693
            static Initializer() { }

            internal static readonly T LazyInstance = GetOrCreate<T>();
        }
    }
    
    [DefaultExecutionOrder(-9999)]
    public abstract class FingletonBase : ScriptableObject
    {
        private static readonly List<FingletonBase> allSingletons = new List<FingletonBase>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod ()
        {
            allSingletons.ForEach(singleton =>
            {
                singleton.CreateInstance();
            });
        }

        protected static T GetOrCreate<T>() where T : FingletonBase {
            Debug.Log("GetOrCreate Instance");
            var instance = allSingletons.FirstOrDefault(s => s.GetType() == typeof(T)) as T;
            instance = instance != null ? instance : CreateInstance<T>();
            instance.Initialize();
            return instance;
        }
        
        protected virtual void Initialize() {}

        protected abstract void CreateInstance();

        private void OnEnable()
        {
#if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }
#endif
            allSingletons.RemoveAll(s => s == null);
            allSingletons.Add(this); 
#if UNITY_EDITOR
            UnityEditor.PlayerSettings.SetPreloadedAssets(UnityEditor.PlayerSettings.GetPreloadedAssets().Where(asset => asset.GetInstanceID() != this.GetInstanceID()).Append(this).ToArray());
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.PlayerSettings.SetPreloadedAssets(UnityEditor.PlayerSettings.GetPreloadedAssets().Where(asset => asset != null).ToArray());
#endif
        }
    }
}