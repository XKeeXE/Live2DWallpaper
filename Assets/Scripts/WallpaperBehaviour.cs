using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WallpaperBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _UI; // edit this later?
    [SerializeField] private float _maxHoldLongPressSeconds = 0.6f; // max seconds needed to hold to do the long press event
    [SerializeField] private float _maxDoubleClickResetSeconds = 0.2f; // max seconds to count double click to do the double click event
    [SerializeField] private float _maxShowUISeconds = 5f; // max seconds for UI to show before hiding

    private bool _UITurnedOn = false;
    private int _touches = 0; // to count how many touches were made in a period of time
    private float _currentSeconds = 0f; 
    private float _pressedTimer, _lastClickedTimer = 0f;

    private PlayerInput _playerInput; 
    private InputAction _touchPressAction;

    private Coroutine _showUI;

    public UnityEvent ActivateEventUI;
    public UnityEvent ActivateEventChara; 

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _touchPressAction = _playerInput.actions["TouchRemu"];
    }

    private void OnEnable() {
        _touchPressAction.performed += OnTouchActionPerformed;

    }

    private void OnDisable() {
        _touchPressAction.performed -= OnTouchActionPerformed;
    }

    // Normal touch input
    // When a touch has been inserted
    private void OnTouchActionPerformed(InputAction.CallbackContext context) {
        _currentSeconds = 0f; // reset current time
        ActivateEventUI?.Invoke(); 
        StartCoroutine(LongPress()); // start the coroutine for long press
        StartCoroutine(DoubleClick()); // start the coroutine for double click
    }
    // After executing long press action
    private void OnActivateEventUI() {
        if (_UITurnedOn == false) {
            HandleShowUI(); // if the UI was hidden then show it
        } else {
            _currentSeconds = _maxShowUISeconds; // else close it by setting the auto hide 
        }
    }

    // After executing double click
    private void OnActivateEventChara() {
        ActivateEventChara?.Invoke();
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

    // For キャラ
    private IEnumerator DoubleClick() {
        _touches += 1; // add touches
        if (_touches == 2) { // when touched twice then execute the event
            Debug.Log("Double Click Activated");
            OnActivateEventChara(); // active 
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

}