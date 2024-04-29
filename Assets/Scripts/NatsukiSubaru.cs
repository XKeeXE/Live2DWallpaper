using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatsukiSubaru : MonoBehaviour
{

    [SerializeField] private GameObject _natsukiSubaru;
    [SerializeField] private AudioClip[] _subaruAudioClip;

    private int _maxNum, _randindex, _previousIndex;
    private string _trigger = "";

    private Animator _subaruAnim;
    private AudioSource _subaruAudioSource;
    private Coroutine _StartTriggers;
    private Coroutine _StartSubaruVoice;

    private void Awake() {
        _subaruAnim = GetComponent<Animator>();
        _subaruAudioSource = GetComponent<AudioSource>();
    }

    public void HandleStartTrigger() {
        if (_natsukiSubaru.activeSelf == false) {
            _StartTriggers = null;
            return;
        }
        _StartTriggers ??= StartCoroutine(StartTrigger());
    }

    private IEnumerator StartTrigger() {
        SubaruMotion(); // start the スバル motion
        yield return new WaitForSecondsRealtime(_subaruAudioSource.clip.length);
        StopTrigger(); // set to idle animation
        _StartTriggers = null; // reset the animation trigger
    }

    public string RandomTrigger() {
        _maxNum = _subaruAnim.parameterCount-1; // get the max allowed number
        _randindex = UnityEngine.Random.Range(1, _maxNum); // get a random int
        _trigger = _subaruAnim.GetParameter(_randindex).name; // get the animation name trigger
        Debug.Log(_trigger);
        return _trigger;
    }

    private void StopTrigger() {
        _subaruAnim.SetTrigger("normalTrigger"); // the idle animation
    }

    private void SubaruMotion() {
        _subaruAnim.SetTrigger(RandomTrigger()); // a random animation
    }

    public void HandleStartSubaruVoice() {
        // if (voiceIndex < _remuAudioClip.Length) {
        //     // Debug.Log("Remu is on voice clip: " + voiceIndex);
        //     return;
        // }
        if (_natsukiSubaru.activeSelf == false) {
            _StartSubaruVoice = null;
            return;
        }
        _StartSubaruVoice ??= StartCoroutine(StartSubaruVoice());
        // Debug.Log("No more voice clips to play as it is set on: No Repeat");
    }

    private void SubaruVoiceClip() {
        // Grab the audio from somewhere
        // Random
        _randindex = UnityEngine.Random.Range(0, _subaruAudioClip.Length); // Pick random clip from array
        // Debug.Log(_subaruAudioSource.clip.length);
        // No repetition
        // _remuAudioSource.clip = _remuAudioClip[voiceIndex]; // insert the clip
        // voiceIndex++;
        while (_randindex == _previousIndex) {
            _randindex = UnityEngine.Random.Range(0, _subaruAudioClip.Length); // Pick random clip from array
        }
        _subaruAudioSource.clip = _subaruAudioClip[_randindex]; // insert the clip
        // play the first voice clip for the intro
        // if (voiceIndex < 1) {
        //     _remuAudioSource.clip = _remuAudioClip[0];
        //     voiceIndex++;
        // }
        _previousIndex = _randindex;
    }

    private IEnumerator StartSubaruVoice() {
        SubaruVoiceClip();
        _subaruAudioSource.Play(); // Play the selected audio
        yield return new WaitForSeconds(_subaruAudioSource.clip.length); // Wait for スバル voice to finish
        _subaruAudioSource.clip = null; // reset the audio
        _StartSubaruVoice = null; // reset the coroutine
    }
}
