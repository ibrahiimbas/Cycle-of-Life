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
 
 [SerializeField] private Pattern orionPattern;
 [SerializeField] private Pattern tetrisPattern;
 [SerializeField] private Pattern pentominoPattern;
 [SerializeField] private Pattern pufferFishPattern;
 [SerializeField] private Pattern snackerPattern;
 [SerializeField] private Pattern wilmaPattern;
 
 [SerializeField] private Button orionButton;
 [SerializeField] private Button tetrisButton;
 [SerializeField] private Button pentominoButton;
 [SerializeField] private Button pufferFishButton;
 [SerializeField] private Button snackerButton;
 [SerializeField] private Button wilmaButton;
 [SerializeField] private Button exitButton;
 [SerializeField] private Button resumeButton;
 [SerializeField] private Button pauseButton;
 [SerializeField] private Button rulesButton;
 [SerializeField] private Button patternMenuOpenButton;
 [SerializeField] private Button patternMenuCloseButton;
 
 [SerializeField] private TextFlicker textFlicker;

 public GameBoard gameBoard;

 public void Start()
 {
    // clickeEffectResume.Stop();
     //pauseText.enabled = false;
     pausePanel.SetActive(false);
     currentPatterntxt.text = "Pentomino-R";
     restartButton.onClick.AddListener(RestartSimulation);
     orionButton.onClick.AddListener(SelectOrionPattern);
     tetrisButton.onClick.AddListener(SelectTetrisPattern);
     pentominoButton.onClick.AddListener(SelectPentominoPattern);
     pufferFishButton.onClick.AddListener(SelectPufferFishPattern);
     snackerButton.onClick.AddListener(SelectSnackerPattern);
     wilmaButton.onClick.AddListener(SelectWilmaPattern);
     exitButton.onClick.AddListener(OnPressedExitButton);
     resumeButton.onClick.AddListener(ResumeGame);
     pauseButton.onClick.AddListener(PauseGame);
     rulesButton.onClick.AddListener(JumptoRulesScene);
     patternMenuOpenButton.onClick.AddListener(OpenPatternMenu);
     patternMenuCloseButton.onClick.AddListener(ClosePatternMenu);
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

 private void SelectWilmaPattern()
 {
     gameBoard.pattern = wilmaPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = "Wilma";
     currentPatterntxt.text = currentPattern;
 }
 
 private void SelectSnackerPattern()
 {
     gameBoard.pattern = snackerPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = "Snacker";
     currentPatterntxt.text = currentPattern;
 }
 
 void SelectPufferFishPattern()
 {
     gameBoard.pattern = pufferFishPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = "Puffer Fish";
     currentPatterntxt.text = currentPattern;
 }
 
 void SelectOrionPattern()
 {
     gameBoard.pattern = orionPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = "Orion-2";
     currentPatterntxt.text = currentPattern;
 }
 
 void SelectPentominoPattern()
 {
     gameBoard.pattern = pentominoPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = "Pentomino-R";
     currentPatterntxt.text = currentPattern;
 }

 void SelectTetrisPattern()
 {
     gameBoard.pattern = tetrisPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
     currentPattern = "Tetris";
     currentPatterntxt.text = currentPattern;
 }

 void OnPressedExitButton()
 {
     Application.Quit();
 }
 
}
