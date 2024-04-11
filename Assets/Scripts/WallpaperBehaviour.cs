using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WallpaperBehaviour : MonoBehaviour
{
    [SerializeField] private NatsukiRemu _natsukiRemu;
    [SerializeField] private GameObject _UI; // edit this later?
    [SerializeField] private float _maxHoldLongPressSeconds = 0.6f; // max seconds needed to hold to do the long press event
    [SerializeField] private float _maxDoubleClickResetSeconds = 0.2f; // max seconds to count double click to do the double click event
    [SerializeField] private float _maxShowUISeconds = 5f; // max seconds for UI to show before hiding
    // [SerializeField] private int _heartbeatSensor = 3;

    private bool _UITurnedOn = false;
    private int _touches = 0; // to count how many touches were made in a period of time
    // private int _nextHour;
    private float _currentSeconds = 0f; 
    private float _pressedTimer, _lastClickedTimer = 0f;

    private AndroidPlugins _androidPlugins; // android functions
    private AudioSource _bgm; // background track
    private PlayerInput _playerInput; 
    private InputAction _touchPressAction;

    private Coroutine _showUI;
    private Coroutine _saveBattery;
    private Coroutine _heartbeat;

    // public delegate void OnDeviceOff();
    // public static event OnDeviceOff onDeviceOff;

    public UnityEvent ActivateEventUI;
    public UnityEvent ActivateEventRemu; 

    private void Awake() {
        _androidPlugins = GetComponent<AndroidPlugins>();
        _bgm = GetComponent<AudioSource>();
        _playerInput = GetComponent<PlayerInput>();
        _touchPressAction = _playerInput.actions["TouchRemu"];

        // #if UNITY_ANDROID && !UNITY_EDITOR
        // AutoAdjustHour();
        // #endif
    }

    private void Update() {
        // If not connected try again to connect to the server x seconds
        // HandleHeartbeat(); // Every X seconds
    }

    private void OnEnable() {
        _touchPressAction.performed += OnTouchActionPerformed;
        // onDeviceOff += SaveBattery;

    }

    private void OnDisable() {
        _touchPressAction.performed -= OnTouchActionPerformed;
        // onDeviceOff -= SaveBattery;
    }

    // Normal touch input
    // When a touch has been inserted
    private void OnTouchActionPerformed(InputAction.CallbackContext context) {
        _currentSeconds = 0f; // reset current time
        ActivateEventUI?.Invoke(); 
        StartCoroutine(LongPress()); // start the coroutine for long press
        StartCoroutine(DoubleClick()); // start the coroutine for double click
    }

    // public void AutoBrightnessState() {
    //     if (_autoBrightnessState == false) {
    //         _autoBrightnessState = true;
    //     } else {
    //         _autoBrightnessState = false;
    //     }
    //     _androidPlugins.ShowToast(_autoBrightnessState.ToString());
    // }

    // After executing long press action
    private void OnActivateEventUI() {
        // Debug.Log(System.DateTime.Now.ToLongTimeString());
        // Debug.Log(System.DateTime.Now.ToLongDateString());
        if (_UITurnedOn == false) {
            HandleShowUI(); // if the UI was hidden then show it
        } else {
            _currentSeconds = _maxShowUISeconds; // else close it by setting the auto hide 
        }
    }

    // After executing double click
    private void OnActivateEventRemu() {
        ActivateEventRemu?.Invoke();
    }

    
    private IEnumerator StartShowUI() {
        _UITurnedOn = true; // set the UI as turned on
        _UI.SetActive(true); // show the UI
        while (_currentSeconds < _maxShowUISeconds) { // while the current seconds is less than the determined max UI seconds then keep adding seconds to auto close it
            _currentSeconds += Time.unscaledDeltaTime;
            yield return null;
        }
        _UITurnedOn = false; // set the UI as turned off
        _UI.SetActive(false); // hide the UI
        _showUI = null; // reset the coroutine
    }

    // For レム
    private IEnumerator DoubleClick() {
        _touches += 1; // add touches
        if (_touches == 2) { // when touched twice then execute the event
            Debug.Log("Double Click Activated");
            OnActivateEventRemu(); // active レム
            _touches = 0; // reset the touches
            _lastClickedTimer = 0; // reset the timer of the last touch
        }
        // while the seconds of the last click is less than the determined max seconds then keep adding to the last click timer
        while (_lastClickedTimer < _maxDoubleClickResetSeconds) {
            _lastClickedTimer += Time.unscaledDeltaTime;
            // if the seconds of the last click is higher than or equal to the determined max seconds then stop the double click coroutine
            if (_lastClickedTimer >= _maxDoubleClickResetSeconds) {
                _touches = 0; // reset the touches
                _lastClickedTimer = 0; // reset the timer of the last touch
                break;
            }
            yield return null;
        }
    }

    // For UI
    private IEnumerator LongPress() {
        while (_touchPressAction.IsPressed() == true) { // while the press is being active
            _pressedTimer += Time.unscaledDeltaTime; // add seconds to the pressed timer
            // if the pressed seconds is equal or more than the determined max long press seconds then activate the event
            if (_pressedTimer >= _maxHoldLongPressSeconds) {
                Debug.Log("Long Press Activated");
                OnActivateEventUI();
                break;
            }
            yield return null;
        }
        _pressedTimer = 0f; // reset the press timer
    }

    private void HandleShowUI() {
        _showUI ??= StartCoroutine(StartShowUI());
    }

    // private void AutoAdjustHour() {
    //     _nextHour = _androidPlugins.GetCurrentHour()+1;
    //     if (_nextHour >= 24) {
    //         _nextHour = 0;
    //     }
    // }

#if UNITY_ANDROID && !UNITY_EDITOR
    // private void Update() {
        // HandleHeartbeat();
    // }

    // // SaveBattery safe check to run only once per heartbeat
    // private void HandleSaveBattery() {
    //     _saveBattery ??= StartCoroutine(StartSaveBattery());
    // }

    // Heartbeat safe check to run only once
    // private void HandleHeartbeat() {
    //     _heartbeat ??= StartCoroutine(Heartbeat());
    // }

    // // Coroutine that will estimate the current activity of the phone
    // private IEnumerator StartSaveBattery() {
    //     // if (_androidPlugins.GetInteractive() == true) {
    //     //     _saveBattery = null;
    //     //     yield break;
    //     // }
    //     // _currentSeconds = _maxShowUISeconds;
    //     // var rateManager = RateManager.Instance;
    //     // var updateRateRequest = rateManager.UpdateRate.Request(18);
    //     // var fixedUpdateRateRequest = rateManager.FixedUpdateRate.Request(1);
    //     // var renderIntervalRequest = rateManager.RenderInterval.Request(10);
    //     // _bgm.Play();
    //     while (_androidPlugins.GetInteractive() == false) {
    //         yield return null;
    //     }
    //     // _bgm.Stop();
    //     // updateRateRequest.Dispose();
    //     // fixedUpdateRateRequest.Dispose();
    //     // renderIntervalRequest.Dispose();
    //     _saveBattery = null;
    // }

    // Coroutine to handle the safe check of SaveBattery to activate after x real seconds
    // private IEnumerator Heartbeat() {

    //     // StartSaveBattery();
    //     // if (_androidPlugins.GetInteractive() == false) {
    //     //     // onDeviceOff?.Invoke();
    //     //     // HandleSaveBattery();
    //     // }

    //     // Try to connect to the server
        
    //     // Add lock system?
    //     #if UNITY_ANDROID && !UNITY_EDITOR
    //     AutoBrightness(); // Auto brightness depending on hour
    //     #endif

    //     yield return new WaitForSecondsRealtime(_heartbeatSensor);
    //     _heartbeat = null;
    // }

    // private void AutoBrightness() {
    //     if (_androidPlugins.GetCurrentHour() != _nextHour) {
    //         return;
    //     }
    //     AutoAdjustHour();
    //     if (_autoBrightnessState == false) {
    //         return;
    //     }

    //     // ---
    //     if (_androidPlugins.GetCurrentHour() >= 0 && _androidPlugins.GetCurrentHour() < 4 ) {
            
    //     } else if (_androidPlugins.GetCurrentHour() == 4 || _androidPlugins.GetCurrentHour() == 5) {
        
    //     } else {
            
    //     }
    //     // ---
    // }
#endif
}