using System;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using TMPro;

public class SpeechManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SpeakText(string text, float rate, float pitch, float volume);

    [DllImport("__Internal")]
    private static extern void StopSpeech();

    [DllImport("__Internal")]
    private static extern int IsSpeaking();
    
    [Header("UI Components")]
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private Button speakButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Image idleEmoticon;
    [SerializeField] private Image talkingEmoticon;

    [Header("Sound Settings")]
    [Range(0.1f, 2f)] public float rate   = 0.75f;
    [Range(0f,   2f)] public float pitch  = 0.4f;
    [Range(0f,   1f)] public float volume = 1f;

    private void Start()
    {
        speakButton.onClick.AddListener(() => Speak(textInput.text));
        stopButton.onClick.AddListener(() => Stop());
        idleEmoticon.gameObject.SetActive(true);
        talkingEmoticon.gameObject.SetActive(false);
    }

    public bool IsCurrentlySpeaking => IsSpeaking() == 1;

    public void Speak(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        idleEmoticon.gameObject.SetActive(false);
        talkingEmoticon.gameObject.SetActive(true);

#if UNITY_WEBGL && !UNITY_EDITOR
        SpeakText(text, rate, pitch, volume);
#else
        Debug.Log($"[TTS] Would speak: {text}");
#endif
    }

    public void Stop()
    {
        idleEmoticon.gameObject.SetActive(true);
        talkingEmoticon.gameObject.SetActive(false);
        
#if UNITY_WEBGL && !UNITY_EDITOR
        StopSpeech();
#endif
    }

    public void StopAndResetOutside()
    {
        Stop();
        textInput.text = "Write whatever you want me to read...";
    }

    public void OnSpeechStart() => Debug.Log("[TTS] Started");
    public void OnSpeechEnd()   => Debug.Log("[TTS] Finished");
    public void OnSpeechError(string error) => Debug.LogWarning("[TTS] Error: " + error);
}