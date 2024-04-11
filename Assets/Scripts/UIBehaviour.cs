using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FantomLib;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private Image _ringImage;
    
    public bool ringState;

    private AndroidPlugins _androidPlugins;
    private Color _ringColorOff = new Color(0.4f, 0.4f, 0.4f);
    private Color _ringColorOn = new Color(1f, 1f, 1f);

    private void Awake() {
        _androidPlugins = GetComponent<AndroidPlugins>();

        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_androidPlugins.GetRingVolume() < 15) {
            _ringImage.color = _ringColorOff; // set the icon as the determined off color
            ringState = false; // set the ringtone as turned off
        } else {
            _ringImage.color = _ringColorOn; // set the icon as the determined on color
            ringState = true; // set the ringtone as turned on
        }
        #endif
    }

    // public string SetAlarmTime(string inputDayOfWeek, string inputTime) {
    //     return _alarmList[0];
    // }

    // public void SetDayOfTheWeekButton(int dayOfTheWeek) {
    //     if (_dayOfTheWeekList.Contains(dayOfTheWeek)) {
    //         _dayOfTheWeekList.Remove(dayOfTheWeek);
    //     } else {
    //         _dayOfTheWeekList.Add(dayOfTheWeek);
    //     }
    // }

    // public void test() {
    //     FantomLib.AndroidPlugin.ShowTimePickerDialog("0:11", "test1", "test2", "test3", "test4");
    // }

#if UNITY_ANDROID && !UNITY_EDITOR
    public void AdjustRingVolumeButton() {        
        if (ringState == true) { // Turn off
            _androidPlugins.MuteRingVolume();
            _ringImage.color = _ringColorOff;
            ringState = false;
        } else { // Turn on
            _androidPlugins.AddRingVolume(15, false);
            _ringImage.color = _ringColorOn;
            ringState = true;
        }
    }

    public void GetBrightness() {
        _androidPlugins.ShowToast(_androidPlugins.GetBrightness().ToString());
    }

    public void GetIsMuted() {
        _androidPlugins.ShowToast(_androidPlugins.IsMuted().ToString());
    }

    public void AdjustScreenBrightness(int brightnessValue) {
        _androidPlugins.AdjustScreenBrightness(brightnessValue);
    }
#endif
}