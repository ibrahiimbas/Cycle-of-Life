using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

public class GeneralSettings : MonoBehaviour
{
    public static GeneralSettings Instance { get; private set; }

    [Header("Post Processing")]
    [SerializeField] private Volume globalVolume;

    [Header("Player Preferens Keys")]
    [SerializeField] private string audioPrefKey = "AudioEnabled";
    [SerializeField] private string postProcessingPrefKey = "PostProcessingEnabled";

    private bool isAudioEnabled = true;
    private bool isPostProcessingEnabled = true;

    private List<AudioSource> cachedAudioSources = new List<AudioSource>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadSettings();
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return null;

        FindReferences();
        CacheAudioSources();

        ApplyAudioSettings();
        ApplyPostProcessingSettings();
    }

    void FindReferences()
    {
        if (globalVolume == null)
            globalVolume = FindObjectOfType<Volume>();
    }

    void CacheAudioSources()
    {
        cachedAudioSources.Clear();
        cachedAudioSources.AddRange(FindObjectsOfType<AudioSource>(true));
    }

    // Audio 
    public void SetAudioEnabled(bool enabled)
    {
        if (isAudioEnabled == enabled) return;

        isAudioEnabled = enabled;
        ApplyAudioSettings();
        SaveSettings();
    }

    void ApplyAudioSettings()
    {
        foreach (var source in cachedAudioSources)
        {
            if (source != null)
                source.mute = !isAudioEnabled;
        }

        AudioListener.pause = !isAudioEnabled;
    }

    public bool IsAudioEnabled() => isAudioEnabled;

    // Post-Processing 
    public void SetPostProcessingEnabled(bool enabled)
    {
        if (isPostProcessingEnabled == enabled) return;

        isPostProcessingEnabled = enabled;
        ApplyPostProcessingSettings();
        SaveSettings();
    }

    void ApplyPostProcessingSettings()
    {
        if (globalVolume == null)
            globalVolume = FindObjectOfType<Volume>();

        if (globalVolume != null)
            globalVolume.enabled = isPostProcessingEnabled;
    }

    public bool IsPostProcessingEnabled() => isPostProcessingEnabled;

    // Save and Load System
    void SaveSettings()
    {
        PlayerPrefs.SetInt(audioPrefKey, isAudioEnabled ? 1 : 0);
        PlayerPrefs.SetInt(postProcessingPrefKey, isPostProcessingEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        isAudioEnabled = PlayerPrefs.GetInt(audioPrefKey, 1) == 1;
        isPostProcessingEnabled = PlayerPrefs.GetInt(postProcessingPrefKey, 1) == 1;
    }

    // Scene System
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        StartCoroutine(Reinitialize());
    }

    IEnumerator Reinitialize()
    {
        yield return null;

        FindReferences();
        CacheAudioSources();

        ApplyAudioSettings();
        ApplyPostProcessingSettings();
    }
}