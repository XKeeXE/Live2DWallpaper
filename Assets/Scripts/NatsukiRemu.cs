using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FantomLib;
using System.Numerics;
using System;

/*
ナツキ・レム

keypass = agithian@12

Wishful thinking list:

Take functionality of LM
Connect to a local server where everything about the LLM information gets saved
Know GPS location
Adjust volume for call depending of weekend day and/or hour
open link but in desktop
synced between desktop, phone, and website


AI TODO LIST:
LM (Brain)
レム voice and manners (Personality)
Audio -> Text [input] (Ears)
Text -> Audio [output] (Mouth)
*/

public class NatsukiRemu : MonoBehaviour {

    [SerializeField] private AudioClip[] _remuAudioClip;

    private int _maxNum, _randindex;
    private string _trigger = "";

    private int voiceIndex = 0;

    private AndroidPlugins _androidPlugins;
    private GameObject _remu;
    private Animator _remuAnim;
    private AudioSource _remuAudioSource;
    private Coroutine _StartTriggers;
    private Coroutine _StartRemuVoice;

    public string globalTrigger = ""; // used on ServerBehaviour to get the activated trigger

    enum Expressions {
        normal,
        egao,
        punpun,
        kangaeru,
        suneru,
        nayamu,
        hohoemu,
        LENGTH
    }

    private void Awake() {
        _androidPlugins = GetComponent<AndroidPlugins>();
        _remuAnim = GetComponent<Animator>();
        _remuAudioSource = GetComponent<AudioSource>();
    }

    public void HandleStartTrigger() {
        _StartTriggers ??= StartCoroutine(StartTrigger());
    }

    private IEnumerator StartTrigger() {
        RemuMotion(); // start the レム motion
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
        _maxNum = _remuAnim.parameterCount-6; // get the max allowed number
        _randindex = UnityEngine.Random.Range(1, _maxNum); // get a random int
        _trigger = _remuAnim.GetParameter(_randindex).name; // get the animation name trigger

        // Only when Phone Section
        #if UNITY_ANDROID && !UNITY_EDITOR
        // _androidPlugins.ShowToast(_trigger);
        #endif

        return _trigger;
    }

    private void StopTrigger() {
        _remuAnim.SetTrigger("normalTrigger"); // the idle animation
    }

    private void RemuMotion() {
        globalTrigger = RandomTrigger();
        _remuAnim.SetTrigger(globalTrigger); // a random animation
    }

    public void HandleStartRemuVoice() {
        // if (voiceIndex < _remuAudioClip.Length) {
        //     // Debug.Log("Remu is on voice clip: " + voiceIndex);
        //     return;
        // }
        _StartRemuVoice ??= StartCoroutine(StartRemuVoice());
        // Debug.Log("No more voice clips to play as it is set on: No Repeat");
    }

    private void RemuVoiceClip() {
        // Grab the audio from somewhere
        // Random
        _randindex = UnityEngine.Random.Range(0, _remuAudioClip.Length); // Pick random clip from array
        _remuAudioSource.clip = _remuAudioClip[_randindex]; // insert the clip

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

        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_androidPlugins.GetMediaVolume() <= 2 || _androidPlugins.IsMuted()) { // If volume low or phone muted, yield return
            _StartRemuVoice = null; // reset the voice
            yield break;
        }
        #endif
        
        RemuVoiceClip();
        _remuAudioSource.Play(); // Play the selected audio
        yield return new WaitForSeconds(_remuAudioSource.clip.length); // Wait for レム voice to finish
        _remuAudioSource.clip = null; // reset the audio
        _StartRemuVoice = null; // reset the coroutine
    }
}
