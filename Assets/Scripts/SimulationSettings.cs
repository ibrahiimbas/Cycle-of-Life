using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
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
    
 [SerializeField] private Button restartButton;
 [SerializeField] private TextMeshProUGUI currentPatterntxt;
 
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

 public GameBoard gameBoard;

 public void Start()
 {
     pauseText.enabled = false;
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
 }

 private void JumptoRulesScene()
 {
     ResumeGame();
     SceneManager.LoadScene("RulesScene", LoadSceneMode.Single);
 }
 
 void PauseGame()
 {
     isPaused = !isPaused;
     if (isPaused == true)
     {
         pauseText.enabled = true;
         Time.timeScale = 0f;
     }
 }

 void ResumeGame()
 {
     isPaused = !isPaused;
     if (isPaused == false)
     {
         pauseText.enabled = false;
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
