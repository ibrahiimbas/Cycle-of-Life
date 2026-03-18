using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Globalization;
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
    [SerializeField] private TextMeshProUGUI currentTimeText;
    private string formattedTime;
    
    public void Start()
    {
        backButton.onClick.AddListener(JumpToSimulation);
        exitButton.onClick.AddListener(ExitSimulation); 
    }

    private void Update()
    {
        DateTime currentTime = DateTime.Now;
        formattedTime = currentTime.ToString("hh:mm tt",CultureInfo.InvariantCulture);
        currentTimeText.text = formattedTime;
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
