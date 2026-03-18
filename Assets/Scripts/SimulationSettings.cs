using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SimulationSettings : MonoBehaviour
{
    public Camera mainCamera;
    private string currentPattern;
    private bool isPaused=false;
    public TextMeshProUGUI pauseText;

    [SerializeField] private CameraScript cameraScript;
 [SerializeField] private Button restartButton;
 [SerializeField] private TextMeshProUGUI currentPatterntxt;
 [SerializeField] private GameObject pausePanel;
 [SerializeField] private GameObject pauseBackground;
 [SerializeField] private GameObject selectPatternPanelOpened;
 [SerializeField] private GameObject selectPatternPanelClosed;
 [SerializeField] private GameObject rulesTab;
 
 [System.Serializable]
 public class PatternData
 {
     public string displayName;
     public Pattern pattern;
     public Button button;
 }
 
 [SerializeField] private List<PatternData> patterns = new List<PatternData>();
 
 [SerializeField] private Button orionButton;
 [SerializeField] private Button tetrisButton;
 [SerializeField] private Button pentominoButton;
 [SerializeField] private Button pufferFishButton;
 [SerializeField] private Button snackerButton;
 [SerializeField] private Button wilmaButton;
 [SerializeField] private Button achimsP16Button;
 [SerializeField] private Button exitButton;
 [SerializeField] private Button resumeButton;
 [SerializeField] private Button pauseButton;
 [SerializeField] private Button rulesButton;
 [SerializeField] private Button patternMenuOpenButton;
 [SerializeField] private Button patternMenuCloseButton;
 [SerializeField] private Button rulesTabCloseButton;
 
 [SerializeField] private TextFlicker textFlicker;

 public GameBoard gameBoard;

 public void Start()
 {
    // clickeEffectResume.Stop();
     //pauseText.enabled = false;
     pausePanel.SetActive(false);
     currentPatterntxt.text = "Pentomino-R";
     restartButton.onClick.AddListener(RestartSimulation);
     exitButton.onClick.AddListener(JumptoRulesScene);
     resumeButton.onClick.AddListener(ResumeGame);
     pauseButton.onClick.AddListener(PauseGame);
     rulesButton.onClick.AddListener(OpenRulesTab);
     patternMenuOpenButton.onClick.AddListener(OpenPatternMenu);
     patternMenuCloseButton.onClick.AddListener(ClosePatternMenu);
     rulesTabCloseButton.onClick.AddListener(CloseRulesTab);
     
     // Pattern Setup
     foreach (var patternData in patterns)
     {
         patternData.button.onClick.AddListener(() => SelectPattern(patternData.pattern, patternData.displayName));
     }
 }

 private void OpenRulesTab()
 {
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
     patternMenuOpenButton.interactable = false;
     cameraScript.speedSlider.interactable = false;
     cameraScript.zoomSlider.interactable = false;
     ClosePatternMenu();
     
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
     patternMenuOpenButton.interactable = true;
     cameraScript.speedSlider.interactable = true;
     cameraScript.zoomSlider.interactable = true;
     ClosePatternMenu();
     
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
     selectPatternPanelClosed.SetActive(false);
     selectPatternPanelOpened.SetActive(true);
 }

 private void ClosePatternMenu()
 {
     selectPatternPanelOpened.SetActive(false);
     selectPatternPanelClosed.SetActive(true);
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
     patternMenuOpenButton.interactable = false;
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
 
}
