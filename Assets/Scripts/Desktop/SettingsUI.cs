using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public Toggle audioOnToggle;
    public Toggle audioOffToggle;

    public Toggle ppOnToggle;
    public Toggle ppOffToggle;

    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private TMP_Text resolutionText;

    private Resolution[] allResolutions;
    private int selectedResolution;
    private List<Resolution> selectedResolutionList = new List<Resolution>();

    void Start()
    {
        StartCoroutine(InitializeDelayed());
        
#if UNITY_WEBGL
        if (resolutionDropDown != null) resolutionDropDown.gameObject.SetActive(false);
        if (resolutionText != null)    resolutionText.gameObject.SetActive(false);
#else
        GetAllResolutions();
        resolutionDropDown.onValueChanged.AddListener((v) => ChangeResolution());
#endif
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

#if !UNITY_WEBGL
    private void GetAllResolutions()
    {
        allResolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        int currentResolutionIndex = 0;

        int savedWidth = GeneralSettings.Instance.GetSavedResolutionWidth();
        int savedHeight = GeneralSettings.Instance.GetSavedResolutionHeight();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            newRes = allResolutions[i].width.ToString() + " x " + allResolutions[i].height.ToString();

            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(allResolutions[i]);

                if (allResolutions[i].width == savedWidth && allResolutions[i].height == savedHeight)
                    currentResolutionIndex = selectedResolutionList.Count - 1;
            }
        }

        resolutionDropDown.AddOptions(resolutionStringList);
        resolutionDropDown.SetValueWithoutNotify(currentResolutionIndex);
        Screen.SetResolution(selectedResolutionList[currentResolutionIndex].width, selectedResolutionList[currentResolutionIndex].height, true);
    }

    private void ChangeResolution()
    {
        selectedResolution = resolutionDropDown.value;
        var res = selectedResolutionList[selectedResolution];
        GeneralSettings.Instance.SetResolution(res.width, res.height);
    }
#endif
        
}