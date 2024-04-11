using System;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Scripts.Managers
{
    public class LiveWallpaperMonoInjecterDroid : MonoBehaviour
    {
        private static LiveWallpaperMonoInjecterDroid _instance;
        public static LiveWallpaperMonoInjecterDroid Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("Creating MonoInjecter");
                    GameObject injecterGO = new GameObject()
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    _instance = injecterGO.AddComponent<LiveWallpaperMonoInjecterDroid>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        
        public event Action FixedUpdateEvent = () => { };
        public event Action UpdateEvent = () => { };
        public event Action LateUpdateEvent = () => { };

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            } else {
                _instance = this;
            }
        }
        
        #region Update Callbacks

        private void FixedUpdate() {
            FixedUpdateEvent();
        }
        
        private void Update() {
            UpdateEvent();
        }

        private void LateUpdate() {
            LateUpdateEvent();
        }

        #endregion
        
    }
}