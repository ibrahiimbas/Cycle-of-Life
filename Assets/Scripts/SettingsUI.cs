using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsUI : MonoBehaviour
{
    public Toggle audioOnToggle;
    public Toggle audioOffToggle;

    public Toggle ppOnToggle;
    public Toggle ppOffToggle;

    void Start()
    {
        StartCoroutine(InitializeDelayed());
    }

    IEnumerator InitializeDelayed()
    {
        // Wait 1 frame for change UI toggles
        yield return null;

        var settings = GeneralSettings.Instance;

        if (settings == null)
        {
            Debug.LogError("GeneralSettings not found!");
            yield break;
        }

        // Clear listeners
        audioOnToggle.onValueChanged.RemoveAllListeners();
        audioOffToggle.onValueChanged.RemoveAllListeners();
        ppOnToggle.onValueChanged.RemoveAllListeners();
        ppOffToggle.onValueChanged.RemoveAllListeners();

        // Override settings
        bool audioEnabled = settings.IsAudioEnabled();
        audioOnToggle.SetIsOnWithoutNotify(audioEnabled);
        audioOffToggle.SetIsOnWithoutNotify(!audioEnabled);

        bool ppEnabled = settings.IsPostProcessingEnabled();
        ppOnToggle.SetIsOnWithoutNotify(ppEnabled);
        ppOffToggle.SetIsOnWithoutNotify(!ppEnabled);

        // Add listeners
        audioOnToggle.onValueChanged.AddListener((v) =>
        {
            if (v) settings.SetAudioEnabled(true);
        });

        audioOffToggle.onValueChanged.AddListener((v) =>
        {
            if (v) settings.SetAudioEnabled(false);
        });

        ppOnToggle.onValueChanged.AddListener((v) =>
        {
            if (v) settings.SetPostProcessingEnabled(true);
        });

        ppOffToggle.onValueChanged.AddListener((v) =>
        {
            if (v) settings.SetPostProcessingEnabled(false);
        });
    }
}