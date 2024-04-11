using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FantomLib;

/*
ナツキ・レム

keypass = agithian@12

Take functionality of LM
Connect to a local server where everything about the LLM information gets saved
Know GPS location
Subir volume for call depending of weekend day and/or hour
open link but in desktop


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
        _randindex = Random.Range(1, _maxNum); // get a random int
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
        _StartRemuVoice ??= StartCoroutine(StartRemuVoice());
    }

    private void RemuVoiceClip() {
        // Grab the audio from somewhere
        _randindex = Random.Range(0, _remuAudioClip.Length); // Pick random clip from array
        _remuAudioSource.clip = _remuAudioClip[_randindex]; // insert the clip
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
