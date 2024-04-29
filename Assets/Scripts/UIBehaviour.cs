using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FantomLib;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private Image _ringImage;
    [SerializeField] private Image _micImage;
    
    private bool ringState;
    private bool micState = false;

    private AndroidPlugins _androidPlugins;
    private Color _colorOff = new Color(0.4f, 0.4f, 0.4f);
    private Color _colorOn = new Color(1f, 1f, 1f);

    private void Awake() {
        _androidPlugins = GetComponent<AndroidPlugins>();

        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_androidPlugins.GetRingVolume() < 15) {
            _ringImage.color = _colorOff; // set the icon as the determined off color
            ringState = false; // set the ringtone as turned off
        } else {
            _ringImage.color = _colorOn; // set the icon as the determined on color
            ringState = true; // set the ringtone as turned on
        }
        #endif
    }

    public void MicControllerButton() {
        if (micState == true) { // disable voice clips
            _micImage.color = _colorOff;
            micState = false;
        } else {
            _micImage.color = _colorOn;
            micState = true;
        }
        Debug.Log("Voice clips are: " + micState);
    }

    public void AdjustRingVolumeButton() {        
        if (ringState == true) { // Turn off

            #if UNITY_ANDROID && !UNITY_EDITOR
            _androidPlugins.MuteRingVolume();
            #endif

            _ringImage.color = _colorOff;
            ringState = false;
        } else { // Turn on

            #if UNITY_ANDROID && !UNITY_EDITOR
            _androidPlugins.AddRingVolume(15, false);
            #endif

            _ringImage.color = _colorOn;
            ringState = true;
        }
        Debug.Log("Ring volume is: " + ringState);

    }

#if UNITY_ANDROID && !UNITY_EDITOR
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
