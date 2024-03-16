using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SimulationSettings : MonoBehaviour
{
    public Camera mainCamera;
    private string currentPattern;
    
 [SerializeField] private Button restartButton;
 [SerializeField] private TextMeshProUGUI currentPatterntxt;
 
 [SerializeField] private Pattern orionPattern;
 [SerializeField] private Pattern tetrisPattern;
 [SerializeField] private Pattern pentominoPattern;
 [SerializeField] private Pattern pufferFishPattern;
 [SerializeField] private Pattern snackerPattern;
 
 [SerializeField] private Button orionButton;
 [SerializeField] private Button tetrisButton;
 [SerializeField] private Button pentominoButton;
 [SerializeField] private Button pufferFishButton;
 [SerializeField] private Button snackerButton;

 public GameBoard gameBoard;

 public void Start()
 {
     restartButton.onClick.AddListener(RestartSimulation);
     orionButton.onClick.AddListener(SelectOrionPattern);
     tetrisButton.onClick.AddListener(SelectTetrisPattern);
     pentominoButton.onClick.AddListener(SelectPentominoPattern);
     pufferFishButton.onClick.AddListener(SelectPufferFishPattern);
     snackerButton.onClick.AddListener(SelectSnackerPattern);
 }

 private void RestartSimulation()
 {
     FindObjectOfType<GameBoard>().Restart();
     mainCamera.transform.position = new Vector3(0, 0, -10);
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
 
}
