using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Whisper;
// using Whisper.Utils;

// https://github.com/Macoron/whisper.unity.git?path=/Packages/com.whisper.unity

public class SpeechRecognition : MonoBehaviour
{
    // public WhisperManager whisper;
    // public MicrophoneRecord microphoneRecord;

    // private WhisperStream _stream;

    // private async void Start() {
    //     _stream = await whisper.CreateStream(microphoneRecord);
    //     // _stream.OnResultUpdated += OnResult;
    //     _stream.OnSegmentUpdated += OnSegmentUpdated;
    //     _stream.OnSegmentFinished += OnSegmentFinished;
    //     _stream.OnStreamFinished += OnFinished;

    //     microphoneRecord.OnRecordStop += OnRecordStop;
    // }

    // public void OnRecordPressed() {
    //     print(microphoneRecord.IsRecording);
    //     if (!microphoneRecord.IsRecording) {
    //         _stream.StartStream();
    //         microphoneRecord.StartRecord();
    //     } else
    //         microphoneRecord.StopRecord();
    //     print(microphoneRecord.IsRecording ? "Stop" : "Record");
    // }
    
    // private void OnRecordStop(AudioChunk recordedAudio) {
    //     print("Stopped recording");
    // }

    // private void OnResult(string result) {
    //     // text.text = result;
    //     // UiUtils.ScrollDown(scroll);
    //     print(result);
    // }
    
    // private void OnSegmentUpdated(WhisperResult segment) {
    //     print($"Segment updated: {segment.Result}");
    // }
    
    // private void OnSegmentFinished(WhisperResult segment) {
    //     print($"Segment finished: {segment.Result}");
    // }
    
    // private void OnFinished(string finalResult) {
    //     print("Stream finished!");
    // }
}
