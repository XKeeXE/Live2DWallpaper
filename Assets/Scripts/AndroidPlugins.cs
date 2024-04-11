using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidPlugins : MonoBehaviour
{
    private const string ANDROID_SYSTEM = "com.natsuki.unityplugin.PluginInstance";

    public int GetRingVolume() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getRingVolume", context);
                }
            }
        }
    }

    // Min 0, Max 15
    public int GetMediaVolume() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getMediaVolume", context);
                }
            }
        }
    }

    public int GetBrightness() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getBrightness", context);
                }
            }
        }
    }

    public void AddRingVolume(int amount, bool showUI) {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        androidSystem.CallStatic("addRingVolume", context, amount, showUI);
                }
            }
        }
    }

    public void MuteRingVolume() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        androidSystem.CallStatic("muteRingVolume", context);
                }
            }
        }
    }

    public bool GetInteractive() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<bool>("getInteractive", context);
                }
            }
        }
    }

    public bool IsMuted() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<bool>("isMuted", context);
                }
            }
        }
    }

    public void ShowToast(string message) {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
        context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            androidSystem.CallStatic(
                "showToast",
                context,
                message
            );
        }));
    }

    public int GetCurrentHour() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getCurrentHour", context);
                }
            }
        }
    }

    public int GetCurrentMinute() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getCurrentMinute", context);
                }
            }
        }
    }

    public int GetCurrentSecond() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getCurrentSecond", context);
                }
            }
        }
    }

    public int GetCurrentDate() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getCurrentDate", context);
                }
            }
        }
    }

    public int GetCurrentMonth() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getCurrentMonth", context);
                }
            }
        }
    }

    public int GetCurrentYear() {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM)) {
                        return androidSystem.CallStatic<int>("getCurrentYear", context);
                }
            }
        }
    }
 
    public void AdjustScreenBrightness(int value) {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
        context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            androidSystem.CallStatic(
                "adjustScreenBrightness",
                context,
                value
            );
        }));
    }
}
