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
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle; 

public class RulesSceneScript : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeNotepadTabButton;
    [SerializeField] private Toggle notepadBottomToggle;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private GameObject notepadTab;
    [SerializeField] private Image notepadImage;
    [SerializeField] private Sprite notepadOpenSprite;
    [SerializeField] private Sprite notepadClosedSprite;
    [SerializeField] private TextMeshProUGUI notepadTabHeaderText;
    
    private string formattedTime;
    private Color originalHeaderColor;
    [SerializeField] private Color inactiveHeaderColor;
    
    public void Start()
    {
        backButton.onClick.AddListener(JumpToSimulation);
        exitButton.onClick.AddListener(ExitSimulation);
        closeNotepadTabButton.onClick.AddListener(CloseNotepadTab);
        notepadBottomToggle.onValueChanged.AddListener(OnNotepadToggleChanged);
        originalHeaderColor = notepadTabHeaderText.color;
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

    private void CloseNotepadTab()
    {
        notepadTab.SetActive(false);
        notepadBottomToggle.gameObject.SetActive(false);
    }
    
    private void OnNotepadToggleChanged(bool isOn)
    {
        if (isOn)
        {
            if (notepadImage != null && notepadOpenSprite != null)
            {
                notepadImage.sprite = notepadOpenSprite;
                notepadTabHeaderText.color = originalHeaderColor;
            }
        }
        else
        {
            if (notepadImage != null && notepadClosedSprite != null)
            {
                notepadImage.sprite = notepadClosedSprite;
                notepadTabHeaderText.color = inactiveHeaderColor;
            }
        }
    }
}
