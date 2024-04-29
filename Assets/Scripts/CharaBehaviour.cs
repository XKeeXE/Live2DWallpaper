using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FantomLib;
using System.Numerics;
using System;

public class CharaBehaviour : MonoBehaviour {

    [SerializeField] private AudioClip[] _AudioClips;

    private int _maxNum, _randindex;
    private string _trigger = "";

    private Animator _charaAnim;
    private AudioSource _charaAudioSource;
    private Coroutine _StartTriggers;
    private Coroutine _StartCharaVoice;

    private void Awake() {
        _charaAnim = GetComponent<Animator>();
        _charaAudioSource = GetComponent<AudioSource>();
    }

    public void HandleStartTrigger() {
        _StartTriggers ??= StartCoroutine(StartTrigger());
    }

    private IEnumerator StartTrigger() {
        CharaMotion(); // start the motion
        yield return new WaitForSecondsRealtime(2);
        StopTrigger(); // set to idle animation
        yield return new WaitForSecondsRealtime(3);
        _StartTriggers = null; // reset the animation trigger
    }


    /// <summary>
    /// To activate an animation
    /// </summary>
    /// <returns></returns>
    public string RandomTrigger() {
        _maxNum = _charaAnim.parameterCount-6; // get the max allowed number
        _randindex = UnityEngine.Random.Range(1, _maxNum); // get a random int
        _trigger = _charaAnim.GetParameter(_randindex).name; // get the animation name trigger

        return _trigger;
    }

    private void StopTrigger() {
        _charaAnim.SetTrigger("normalTrigger"); // the idle animation
    }

    private void CharaMotion() {
        // globalTrigger = RandomTrigger();
        // _charaAnim.SetTrigger(globalTrigger); // a random animation
    }

    public void HandleStartRemuVoice() {
        // if (voiceIndex < _remuAudioClip.Length) {
        //     // Debug.Log("Remu is on voice clip: " + voiceIndex);
        //     return;
        // }
        _StartCharaVoice ??= StartCoroutine(StartRemuVoice());
        // Debug.Log("No more voice clips to play as it is set on: No Repeat");
    }

    private void CharaVoiceClip() {
        // Grab the audio from somewhere
        // Random
        _randindex = UnityEngine.Random.Range(0, _AudioClips.Length); // Pick random clip from array
        _charaAudioSource.clip = _AudioClips[_randindex]; // insert the clip

        // No repetition
        // _remuAudioSource.clip = _remuAudioClip[voiceIndex]; // insert the clip
        // voiceIndex++;

        // play the first voice clip for the intro
        // if (voiceIndex < 1) {
        //     _remuAudioSource.clip = _remuAudioClip[0];
        //     voiceIndex++;
        // }
    }

    private IEnumerator StartRemuVoice() {
        
        CharaVoiceClip();
        _charaAudioSource.Play(); // Play the selected audio
        yield return new WaitForSeconds(_charaAudioSource.clip.length); // Wait for レム voice to finish
        _charaAudioSource.clip = null; // reset the audio
        _StartCharaVoice = null; // reset the coroutine
    }
}
