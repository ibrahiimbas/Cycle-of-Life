using UnityEngine;
using System.Collections;
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
    [Range(0.1f, 2f)] public float rate   = 0.75f;
    [Range(0f,   2f)] public float pitch  = 0.4f;
    [Range(0f,   1f)] public float volume = 1f;

    public bool IsCurrentlySpeaking => IsSpeaking() == 1;

    private bool _speechEndHandled = false;
    private Coroutine _watchCoroutine;

    private void Start()
    {
        speakButton.onClick.AddListener(() => Speak(textInput.text));
        stopButton.onClick.AddListener(Stop);
        SetEmoticon(isTalking: false);

#if UNITY_WEBGL && !UNITY_EDITOR
        InitSpeech();
#endif
    }

    public void Speak(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        _speechEndHandled = false;
        
        SetEmoticon(isTalking: true);

        if (_watchCoroutine != null)
        {
            StopCoroutine(_watchCoroutine);
            _watchCoroutine = null;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        SpeakText(text, rate, pitch, volume);
        _watchCoroutine = StartCoroutine(WatchSpeechEnd());
#else
        Debug.Log($"[TTS] Would speak: {text}");
#endif
    }

    public void Stop()
    {
        if (_watchCoroutine != null)
        {
            StopCoroutine(_watchCoroutine);
            _watchCoroutine = null;
        }

        _speechEndHandled = true;
        
        SetEmoticon(isTalking: false);

#if UNITY_WEBGL && !UNITY_EDITOR
        StopSpeech();
#endif
    }

    public void StopAndResetOutside()
    {
        Stop();
        textInput.text = "Write whatever you want me to read...";
    }

    private IEnumerator WatchSpeechEnd()
    {
        yield return new WaitForSeconds(0.5f);

        while (IsCurrentlySpeaking)
        {
            yield return new WaitForSeconds(0.3f);
        }

        OnSpeechEnd();
        _watchCoroutine = null;
    }

    private void SetEmoticon(bool isTalking)
    {
        idleEmoticon.gameObject.SetActive(!isTalking);
        talkingEmoticon.gameObject.SetActive(isTalking);
    }

    public void OnSpeechStart() => Debug.Log("[TTS] Started");

    public void OnSpeechEnd()
    {
        if (_speechEndHandled) return;
        _speechEndHandled = true;

        SetEmoticon(isTalking: false);
        Debug.Log("[TTS] Ended");

    }

    public void OnSpeechError(string error)
    {
        if (_watchCoroutine != null)
        {
            StopCoroutine(_watchCoroutine);
            _watchCoroutine = null;
        }

        _speechEndHandled = true;
        
        Debug.LogWarning($"[TTS] Error: {error}");
        SetEmoticon(isTalking: false);
    }
}