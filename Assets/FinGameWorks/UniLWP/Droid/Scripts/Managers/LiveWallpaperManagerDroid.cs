using System;
using System.Threading;
using System.Threading.Tasks;
using FinGameWorks.UniLWP.Droid.Scripts.Datas;
using UnityEngine;

namespace FinGameWorks.UniLWP.Droid.Scripts.Managers
{
    [CreateAssetMenu(fileName = "LiveWallpaper.Manager.Droid.asset", menuName = "FinGameWorks/UniLWP/Droid/LiveWallpaperManager")]
    public class LiveWallpaperManagerDroid : Fingleton<LiveWallpaperManagerDroid>
    {
#if UNITY_ANDROID
        private TaskScheduler taskScheduler;
#if !UNITY_EDITOR
        private AndroidJavaObject nativeLiveWallpaperManager;
#endif

        protected override void Initialize()
        {
            base.Initialize();
            Debug.Log("LiveWallpaperManagerDroid Initialize");
            taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            AndroidJavaClass lwpClass = new AndroidJavaClass("com.justzht.unity.lwp.LiveWallpaperManager");
#if !UNITY_EDITOR
            // access the native manager
            nativeLiveWallpaperManager = lwpClass.CallStatic<AndroidJavaObject>("getInstance");
            // set the event listener so that Unity can receive such events
            Debug.Log("LiveWallpaperManagerDroid (" + nativeLiveWallpaperManager + ") setEventListener");
            nativeLiveWallpaperManager.Call("setEventListener", new LiveWallpaperCallbackDroid());
#endif
        }

        public void ExecuteMainThread(Action action)
        {
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }

        
        #region Native Functions

#if UNITY_ANDROID
        public void OpenPreview(String className)
        {
#if !UNITY_EDITOR
            nativeLiveWallpaperManager.Call("openLiveWallpaperPreview",new String[] {className});
#endif
        }

        public void OpenPreview()
        {
#if !UNITY_EDITOR
            nativeLiveWallpaperManager.Call("openLiveWallpaperPreview");
#endif
        }
        public void LaunchActivity(String className)
        {
#if !UNITY_EDITOR
            nativeLiveWallpaperManager.Call("launchUnityActivity",new String[] {className});
#endif
        }        
        public void LaunchActivity()
        {
#if !UNITY_EDITOR
            nativeLiveWallpaperManager.Call("launchUnityActivity");
#endif
        }
#endif

        #endregion

        #region Native Events

        public delegate void OnInsetsUpdatedDelegate(int left, int top, int right, int bottom);
        public OnInsetsUpdatedDelegate insetsUpdated;
        public static Vector4 insets;

        public delegate void OnWallpaperOffsetsUpdatedDelegate(float xOffset, float yOffset, float xOffsetStep, float yOffsetStep, bool simulated);
        public OnWallpaperOffsetsUpdatedDelegate wallpaperOffsetsUpdated;
        public static Vector4 offset;
        public static bool offsetSimulated;
        
        public delegate void OnDarkModeEnableUpdatedDelegate(bool darkMode);
        public OnDarkModeEnableUpdatedDelegate darkModeEnableUpdated;
        public static bool isDarkModeEnabled;
        
        public delegate void OnScreenDisplayStatusUpdatedDelegate(Enums.ScreenStatus screenStatus);
        public OnScreenDisplayStatusUpdatedDelegate screenDisplayStatusUpdated;
        public static Enums.ScreenStatus screenStatus;
        
        public delegate void OnEnterActivityUpdatedDelegate(bool inActivity);
        public OnEnterActivityUpdatedDelegate enterActivityUpdated;
        public static bool isInActivity;
        
        public delegate void OnServiceIsInPreviewUpdatedDelegate(bool preview);
        public OnServiceIsInPreviewUpdatedDelegate serviceIsInPreviewUpdated;
        public static bool isInServicePreview;

        public delegate void OnSettingsButtonPressedDelegate();
        public OnSettingsButtonPressedDelegate settingsButtonPressed;

        //TODO add android 11 scale value
        #endregion

        /// <summary>
        /// Callback for native events to pass through into Unity. All interface events are redirected to implementations of the manager.
        /// </summary>
        public class LiveWallpaperCallbackDroid : AndroidJavaProxy
        {
            public LiveWallpaperCallbackDroid() : base("com.justzht.unity.lwp.LiveWallpaperListener")
            {
                Debug.Log("LiveWallpaperCallbackDroid init");
            }

            public void InsetsUpdated(int left, int top, int right, int bottom)
            {
                insets = new Vector4(left, top, right, bottom);
                if (Instance == null) { return; }
                Instance.ExecuteMainThread(() =>
                {
                    Instance.insetsUpdated?.Invoke(left, top, right, bottom);
                });
            }

            public void WallpaperOffsetsUpdated(float xOffset, float yOffset, float xOffsetStep, float yOffsetStep, bool simulated)
            {
                offset = new Vector4(xOffset, yOffset, xOffsetStep, yOffsetStep);
                offsetSimulated = simulated;
                if (Instance == null) { return; }
                Instance.ExecuteMainThread(() =>
                {
                    Instance.wallpaperOffsetsUpdated?.Invoke(xOffset,yOffset,xOffsetStep,yOffsetStep,simulated);
                });
            }

            public void ScreenDisplayStatusUpdated(int status)
            {
                Enums.ScreenStatus v = (Enums.ScreenStatus) status;
                screenStatus = v;
                if (Instance == null) { return; }
                Instance.ExecuteMainThread(() =>
                {
                    Instance.screenDisplayStatusUpdated?.Invoke(v);
                });
            }

            public void DarkModeEnableUpdated(bool darkMode)
            {
                isDarkModeEnabled = darkMode;
                if (Instance == null) { return; }
                Instance.ExecuteMainThread(() =>
                {
                    Instance.darkModeEnableUpdated?.Invoke(darkMode);
                });
            }
            
            public void EnterActivityUpdated(bool inActivity)
            {
                Debug.Log("LiveWallpaperManagerDroid EnterActivityUpdated " + inActivity + " Instance exist " + (Instance != null));
                isInActivity = inActivity;
                if (Instance == null) { return; }
                Debug.Log("LiveWallpaperManagerDroid Instance.isInActivity " + isInActivity);
                Instance.ExecuteMainThread(() =>
                {
                    Instance.enterActivityUpdated?.Invoke(inActivity);
                });
            }
            
            public void ServiceIsInPreviewUpdated(bool preview)
            {
                isInServicePreview = preview;
                if (Instance == null) { return; }
                Instance.ExecuteMainThread(() =>
                {
                    Instance.serviceIsInPreviewUpdated?.Invoke(preview);
                });
            }

            public void SettingsButtonPressed()
            {
                if (Instance == null) { return; }
                Instance.ExecuteMainThread(() =>
                {
                    Instance.settingsButtonPressed?.Invoke();
                });
            }

            public void WallpaperZoomUpdated(float zoom)
            {
                if (Instance == null) { return; }
            }
        }
#endif
    }
}