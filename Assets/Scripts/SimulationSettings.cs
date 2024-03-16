using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SimulationSettings : MonoBehaviour
{
    public Camera mainCamera;
 [SerializeField] private Button restartButton;

 [SerializeField] private Pattern acornPattern;
 [SerializeField] private Pattern tetrisPattern;
 [SerializeField] private Pattern pentominoPattern;
 [SerializeField] private Pattern pufferFishPattern;
 [SerializeField] private Pattern snackerPattern;
 
 [SerializeField] private Button acornButton;
 [SerializeField] private Button tetrisButton;
 [SerializeField] private Button pentominoButton;
 [SerializeField] private Button pufferFishButton;
 [SerializeField] private Button snackerButton;

 public GameBoard gameBoard;

 public void Start()
 {
     restartButton.onClick.AddListener(RestartSimulation);
     acornButton.onClick.AddListener(SelectAcornPattern);
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
 }
 
 void SelectPufferFishPattern()
 {
     gameBoard.pattern = pufferFishPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();;
 }
 
 void SelectAcornPattern()
 {
     gameBoard.pattern = acornPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
 }
 
 void SelectPentominoPattern()
 {
     gameBoard.pattern = pentominoPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
 }

 void SelectTetrisPattern()
 {
     gameBoard.pattern = tetrisPattern;
     mainCamera.transform.position = new Vector3(0, 0, -10);
     gameBoard.Restart();
 }
 
}
