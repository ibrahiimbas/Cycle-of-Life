using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulationSettings : MonoBehaviour
{
    public Camera mainCamera;
    private string currentPattern;
    private bool isPaused = false;
    public TextMeshProUGUI pauseText;

    [Header("Script References")]
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private TextFlicker textFlicker;
    [SerializeField] private ThemeManager themeManager;

    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseBackground;
    [SerializeField] private GameObject selectPatternPanelOpened;
    [SerializeField] private GameObject selectPatternPanelClosed;
    [SerializeField] private GameObject rulesTab;
    [SerializeField] private TextMeshProUGUI currentPatterntxt;
    [SerializeField] private GameObject themesPanelOpened;
    [SerializeField] private GameObject themesPanelClosed;
    [Header("Theme Buttons")]
    [SerializeField] private GameObject themeButtonPrefab;
    [SerializeField] private Transform themeButtonContainer;

    [Header("Pattern Buttons")]
    [SerializeField] private GameObject patternButtonPrefab;
    [SerializeField] private Transform patternButtonContainer;
    
    [Header("Scroll Views")]
    [SerializeField] private ScrollRect patternScrollRect;
    [SerializeField] private ScrollRect themeScrollRect;

    [System.Serializable]
    public class PatternData
    {
        public string displayName;
        public Pattern pattern;
    }

    [SerializeField] private List<PatternData> patterns = new List<PatternData>();

    [Header("Control Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button rulesButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button patternMenuOpenButton;
    [SerializeField] private Button patternMenuCloseButton;
    [SerializeField] private Button themeMenuOpenButton;
    [SerializeField] private Button themeMenuCloseButton;
    [SerializeField] private Button rulesTabCloseButton;

    [SerializeField] private AudioSource notifySound;

    private List<Button> patternButtons = new List<Button>();
    private List<Button> themeButtons = new List<Button>();

    public void Start()
    {
        InitializeUI();
        SetupButtons();
        CreatePatternButtons();
        CreateThemeButtons();
    }

    private void InitializeUI()
    {
        pausePanel.SetActive(false);
        rulesTab.SetActive(false);
        selectPatternPanelClosed.SetActive(true);
        selectPatternPanelOpened.SetActive(false);
        
        if (patterns.Count > 0)
        {
            currentPatterntxt.text = patterns[0].displayName;
        }
    }

    private void SetupButtons()
    {
        restartButton.onClick.AddListener(RestartSimulation);
        exitButton.onClick.AddListener(JumptoRulesScene);
        resumeButton.onClick.AddListener(ResumeGame);
        pauseButton.onClick.AddListener(PauseGame);
        rulesButton.onClick.AddListener(OpenRulesTab);
        patternMenuOpenButton.onClick.AddListener(OpenPatternMenu);
        patternMenuCloseButton.onClick.AddListener(ClosePatternMenu);
        themeMenuOpenButton.onClick.AddListener(OpenThemeMenu);
        themeMenuCloseButton.onClick.AddListener(CloseThemeMenu);
        rulesTabCloseButton.onClick.AddListener(CloseRulesTab);
    }

    private void CreatePatternButtons()
    {
        if (patternButtonPrefab == null || patternButtonContainer == null)
        {
            Debug.LogError("Missin pattern button prefab or target container");
            return;
        }

        // Clear existing button if there are any
        foreach (Transform child in patternButtonContainer)
        {
            Destroy(child.gameObject);
        }
        patternButtons.Clear();

        foreach (var patternData in patterns)
        {
            GameObject newButtonObj = Instantiate(patternButtonPrefab, patternButtonContainer);
            
            Button newButton = newButtonObj.GetComponent<Button>();
            
            TextMeshProUGUI buttonText = newButtonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = patternData.displayName;
            }
            
            newButtonObj.name = "pattern_button_" + patternData.displayName;
            
            Pattern capturedPattern = patternData.pattern;
            string capturedName = patternData.displayName;
            newButton.onClick.AddListener(() => SelectPattern(capturedPattern, capturedName));
            
            patternButtons.Add(newButton);
        }
        
        Debug.Log($"{patternButtons.Count} pattern buttons created");
    }

    private void CreateThemeButtons()
    {
        if (themeButtonPrefab == null || themeButtonContainer == null)
        {
            Debug.LogError("Missing theme button prefab or target container");
            return;
        }

        if (themeManager == null)
        {
            Debug.LogError("Theme Manager is not assigned!");
            return;
        }

        foreach (Transform child in themeButtonContainer)
        {
            Destroy(child.gameObject);
        }
        themeButtons.Clear();

        List<Theme> themes = themeManager.GetAllThemes();
        
        for (int i = 0; i < themes.Count; i++)
        {
            Theme theme = themes[i];
            int themeIndex = i;

            GameObject newButtonObj = Instantiate(themeButtonPrefab, themeButtonContainer);
            
            Button newButton = newButtonObj.GetComponent<Button>();

            TextMeshProUGUI buttonText = newButtonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = theme.themeDisplayName;
            }
            
            newButtonObj.name = "theme_button_" + theme.themeDisplayName;
            
            newButton.onClick.AddListener(() => SelectTheme(theme, themeIndex));
            
            themeButtons.Add(newButton);
        }
        
        Debug.Log($"{themeButtons.Count} theme buttons created");
    }
    
    private void SelectTheme(Theme theme, int index)
    {
        if (themeManager != null)
        {
            themeManager.ApplyThemeByIndex(index);
            
            Debug.Log($"Theme selected: {theme.name} at index {index}");
        }
    }
    
    public void AddNewPattern(string displayName, Pattern pattern)
    {
        PatternData newPattern = new PatternData
        {
            displayName = displayName,
            pattern = pattern
        };
        patterns.Add(newPattern);
        
        if (patternButtons.Count > 0)
        {
            CreatePatternButtons();
        }
    }
    
    public void RemovePattern(string displayName)
    {
        patterns.RemoveAll(p => p.displayName == displayName);
        
        if (patternButtons.Count > 0)
        {
            CreatePatternButtons();
        }
    }

 private void OpenRulesTab()
 {
     notifySound.Play();
     if (isPaused == true)
     {
         // For changing button states timeScale would be 1 for a moment
         Time.timeScale = 1f;
     }
     else
     {
         isPaused = !isPaused;
     }
     exitButton.interactable = false;
     rulesButton.interactable = false;
     restartButton.interactable = false;
     pauseButton.interactable = false;
     resumeButton.interactable = false;
     cameraScript.speedSlider.interactable = false;
     cameraScript.zoomSlider.interactable = false;
     ClosePatternMenu();
     CloseThemeMenu();
     patternMenuOpenButton.interactable = false;
     themeMenuOpenButton.interactable = false;
     
     if (isPaused == true)
     {
         rulesTab.SetActive(true);
         pauseBackground.SetActive(true);
         Time.timeScale = 0f;
     }
 }
 
 private void CloseRulesTab()
 {
     if (isPaused == false)
     {
         // don't change state if game is already paused 
     }
     else
     {
         isPaused = !isPaused;
     }
     exitButton.interactable = true;
     rulesButton.interactable = true;
     restartButton.interactable = true;
     resumeButton.interactable = false;
     pauseButton.interactable = true;
     cameraScript.speedSlider.interactable = true;
     cameraScript.zoomSlider.interactable = true;
     ClosePatternMenu();
     CloseThemeMenu();
     patternMenuOpenButton.interactable = true;
     themeMenuOpenButton.interactable = true;
     
     if (isPaused == false)
     {
         rulesTab.SetActive(false);
         pauseBackground.SetActive(false);
         pausePanel.SetActive(false);
         Time.timeScale = 1f;
     }
 }
 
 private void OpenPatternMenu()
 {
     StartCoroutine(ResetScrollRectNextFrame(patternScrollRect));
     selectPatternPanelClosed.SetActive(false);
     selectPatternPanelOpened.SetActive(true);
 }

 private void ClosePatternMenu()
 {
     selectPatternPanelOpened.SetActive(false);
     selectPatternPanelClosed.SetActive(true);
 }

 private void OpenThemeMenu()
 {
     StartCoroutine(ResetScrollRectNextFrame(themeScrollRect));
     themesPanelOpened.SetActive(true);
     themesPanelClosed.SetActive(false);
 }

 private void CloseThemeMenu()
 {
     themesPanelOpened.SetActive(false);
     themesPanelClosed.SetActive(true);
 }
 
 private void JumptoRulesScene()
 {
     ResumeGame();
     SceneManager.LoadScene("RulesScene", LoadSceneMode.Single);
 }
 
 void PauseGame()
 {
     pauseButton.interactable = false;
     resumeButton.interactable = true;
     cameraScript.speedSlider.interactable = false;
     cameraScript.zoomSlider.interactable = false;
     ClosePatternMenu();
     CloseThemeMenu();
     patternMenuOpenButton.interactable = false;
     themeMenuOpenButton.interactable = false;
     isPaused = !isPaused;
     if (isPaused == true)
     {
         pausePanel.SetActive(true);
         pauseBackground.SetActive(true);
         //pauseText.enabled = true;
         //textFlicker.FlickerLoop(pauseText);   // Not working because of time scale!!
         Time.timeScale = 0f;
     }
 }
 
 void ResumeGame()
 {
     resumeButton.interactable = false;
     pauseButton.interactable = true;
     cameraScript.speedSlider.interactable = true;
     cameraScript.zoomSlider.interactable = true;
     patternMenuOpenButton.interactable = true;
     themeMenuOpenButton.interactable = true;
     isPaused = !isPaused;
     if (isPaused == false)
     {
         pausePanel.SetActive(false);
         pauseBackground.SetActive(false);
         //pauseText.enabled = false;
         Time.timeScale = 1f;
     }
 }
 
 private void RestartSimulation()
 {
     FindObjectOfType<GameBoard>().Restart();
     mainCamera.transform.position = new Vector3(0, 0, -10);
 }
 
 private void SelectPattern(Pattern pattern, string displayName)
 {
     gameBoard.pattern = pattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = displayName;
     currentPatterntxt.text = currentPattern;
 }
 
 void OnPressedExitButton()
 {
     Application.Quit();
 }
 
 private IEnumerator ResetScrollRectNextFrame(ScrollRect scrollRect)
 {
     yield return null;
    
     yield return new WaitForEndOfFrame();

     if (scrollRect.vertical)
     {
         scrollRect.verticalNormalizedPosition = 1f;
     }

     else
     {
         scrollRect.horizontalNormalizedPosition = 0f;
     }
    
 }
 
}
