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

    [DllImport("__Internal")]
    private static extern void InitSpeech();

    [Header("UI Components")]
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private Button speakButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Image idleEmoticon;
    [SerializeField] private Image talkingEmoticon;

    [Header("Sound Settings")]
    [Range(0.1f, 2f)] public float rate = 0.75f;
    [Range(0f, 2f)] public float pitch = 0.4f;
    [Range(0f, 1f)] public float volume = 1f;

    private bool _isSpeaking = false;

    private void Start()
    {
        speakButton.onClick.AddListener(() => Speak(textInput.text));
        stopButton.onClick.AddListener(Stop);

        SetEmoticon(false);

#if UNITY_WEBGL && !UNITY_EDITOR
        InitSpeech(); 
#endif

        if (string.IsNullOrEmpty(textInput.text))
            textInput.text = "";
    }

    public void Speak(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        if (text == "") return;

        SetEmoticon(true);
        _isSpeaking = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        SpeakText(text, rate, pitch, volume);
#else
        Debug.Log($"[TTS] Editor mode - Speaking: {text}");
        float duration = Mathf.Clamp(text.Length * 0.05f, 0.5f, 5f);
        Invoke(nameof(SimulateSpeechEnd), duration);
#endif
    }

    public void Stop()
    {
        if (!_isSpeaking) return;

        _isSpeaking = false;
        SetEmoticon(false);

#if UNITY_WEBGL && !UNITY_EDITOR
        StopSpeech();  
#endif
    }

    public void StopAndResetOutside()
    {
        Stop();
        textInput.text = "";
    }

    public void OnSpeechStart()
    {
        Debug.Log("[TTS] Speech started (callback)");
        SetEmoticon(true);
        _isSpeaking = true;
    }

    public void OnSpeechEnd()
    {
        Debug.Log("[TTS] Speech ended (callback)");
        _isSpeaking = false;
        SetEmoticon(false);
    }

    public void OnSpeechError(string error)
    {
        Debug.LogWarning($"[TTS] Speech error: {error}");
        OnSpeechEnd();
    }

    private void SimulateSpeechEnd()
    {
        OnSpeechEnd();
    }

    private void SetEmoticon(bool isTalking)
    {
        if (idleEmoticon != null)
            idleEmoticon.gameObject.SetActive(!isTalking);
        if (talkingEmoticon != null)
            talkingEmoticon.gameObject.SetActive(isTalking);
    }

    private void OnDestroy()
    {
        speakButton?.onClick.RemoveAllListeners();
        stopButton?.onClick.RemoveAllListeners();
    }
}