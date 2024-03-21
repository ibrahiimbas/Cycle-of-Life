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

public class RulesSceneScript : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    public void Start()
    {
        backButton.onClick.AddListener(JumpToSimulation);
        exitButton.onClick.AddListener(ExitSimulation);
    }

    private void JumpToSimulation()
    {
        SceneManager.LoadScene("CycleofLife", LoadSceneMode.Single);
    }

    private void ExitSimulation()
    {
        Application.Quit();
    }
}
