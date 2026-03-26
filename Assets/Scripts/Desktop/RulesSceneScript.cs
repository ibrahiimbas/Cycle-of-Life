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
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle; 

public class RulesSceneScript : MonoBehaviour
{
    [SerializeField] private SystemProperties systemProperties;
    [SerializeField] private Button closeNotepadTabButton;
    [SerializeField] private Toggle notepadBottomToggle;
    [SerializeField] private Button controlPanelButton;
    [SerializeField] private Button closeControlPanelButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button shutDownTabButton;
    [SerializeField] private Button closeShutDownButton;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelShutButton;
    [SerializeField] private Button firstInfoCloseButton;
    [SerializeField] private Button firstInfoOkButton;
    [SerializeField] private Button updateNotepadButton;
    
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private GameObject notepadTab;
    [SerializeField] private GameObject controlPanelTab;
    [SerializeField] private GameObject shutDownSidePanelTab;
    [SerializeField] private GameObject shutDownPanelTab;
    [SerializeField] private GameObject firstInfoPanel;
    [SerializeField] private Image notepadImage;
    [SerializeField] private Sprite notepadOpenSprite;
    [SerializeField] private Sprite notepadClosedSprite;
    [SerializeField] private TextMeshProUGUI notepadTabHeaderText;
    [SerializeField] private TMP_InputField updateNotepadInputText;
    
    private string formattedTime;
    private Color originalHeaderColor;
    [SerializeField] private Color inactiveHeaderColor;
    
    [SerializeField] private AudioSource shutDownAudio;
    [SerializeField] private ClickSoundEffectScript clickSoundEffectObject;
    [SerializeField] private AudioSource notificationAudio;

    private bool isStartTabOpen = false;
    
    private const string NOTEPAD_TEXT_KEY = "NotepadSavedText";
    
    public void Start()
    {
        closeNotepadTabButton.onClick.AddListener(CloseNotepadTab);
        notepadBottomToggle.onValueChanged.AddListener(OnNotepadToggleChanged);
        controlPanelButton.onClick.AddListener(OpenControlPanelTab);
        closeControlPanelButton.onClick.AddListener(CloseControlPanelTab);
        startButton.onClick.AddListener(OpenStartTab);
        shutDownTabButton.onClick.AddListener(OpenShutDownTab);
        closeShutDownButton.onClick.AddListener(CloseShutDownTab);
        cancelShutButton.onClick.AddListener(CloseShutDownTab);
        okButton.onClick.AddListener(OnOKButtonClicked);
        firstInfoCloseButton.onClick.AddListener(FirstInfoPanelClose);
        firstInfoOkButton.onClick.AddListener(FirstInfoPanelClose);
        updateNotepadButton.onClick.AddListener(NotepadOpen);
        
        originalHeaderColor = notepadTabHeaderText.color;
        
        LoadNotepadContent();
        updateNotepadInputText.onValueChanged.AddListener(SaveNotepadContent);
        
        systemProperties.GetSystemProperties();
    }

    private void Update()
    {
        DateTime currentTime = DateTime.Now;
        formattedTime = currentTime.ToString("hh:mm tt",CultureInfo.InvariantCulture);
        currentTimeText.text = formattedTime;
    }
    
    private void OnDestroy()
    {
        if (updateNotepadInputText != null)
        {
            updateNotepadInputText.onValueChanged.RemoveListener(SaveNotepadContent);
        }
    }
    
    private void SaveNotepadContent(string content)
    {
        PlayerPrefs.SetString(NOTEPAD_TEXT_KEY, content);
        PlayerPrefs.Save();
    }
    
    private void LoadNotepadContent()
    {
        if (PlayerPrefs.HasKey(NOTEPAD_TEXT_KEY))
        {
            string savedText = PlayerPrefs.GetString(NOTEPAD_TEXT_KEY);
            updateNotepadInputText.text = savedText;
        }
        else
        {
            
        }
    }
    
    public void ClearNotepadContent()
    {
        PlayerPrefs.DeleteKey(NOTEPAD_TEXT_KEY);
        PlayerPrefs.Save();
        //updateNotepadInputText.text = "";
    }

    private void JumpToSimulation()
    {
        SceneManager.LoadScene("CycleofLife", LoadSceneMode.Single);
    }

    private void ExitSimulation()
    {
        ClearNotepadContent();
        Application.Quit();
    }
    
    private void OnApplicationQuit()
    {
        ClearNotepadContent();
    }

    private void FirstInfoPanelClose()
    {
        notepadImage.sprite = notepadOpenSprite;
        notepadTabHeaderText.color = originalHeaderColor;
        firstInfoPanel.SetActive(false);
    }

    public void FirstInfoPanelOpen()
    {
        notepadImage.sprite = notepadClosedSprite;
        notepadTabHeaderText.color = inactiveHeaderColor;
        notificationAudio.Play();
        firstInfoPanel.SetActive(true);
    }
    
    private void CloseShutDownTab()
    {
        shutDownPanelTab.SetActive(false);
        if (notepadImage != null && notepadOpenSprite != null)
        {
            notepadImage.sprite = notepadOpenSprite;
            notepadTabHeaderText.color = originalHeaderColor;
        }
    }

    private void OpenStartTab()
    {
        if (isStartTabOpen)
        {
            shutDownSidePanelTab.SetActive(false);
            isStartTabOpen = !isStartTabOpen;
        }
        else
        {
            shutDownSidePanelTab.SetActive(true);
            isStartTabOpen = !isStartTabOpen;
        }
    }

    private void OpenShutDownTab()
    {
        shutDownSidePanelTab.SetActive(false);
        shutDownPanelTab.SetActive(true);
        isStartTabOpen = !isStartTabOpen;
        if (notepadImage != null && notepadClosedSprite != null)
        {
            notepadImage.sprite = notepadClosedSprite;
            notepadTabHeaderText.color = inactiveHeaderColor;
        }
    }

    private void OpenControlPanelTab()
    {
        controlPanelTab.SetActive(true);
        controlPanelButton.interactable = false;
        notepadImage.sprite = notepadClosedSprite;
        notepadTabHeaderText.color = inactiveHeaderColor;
    }

    private void CloseControlPanelTab()
    {
        controlPanelTab.SetActive(false);
        controlPanelButton.interactable = true;
        notepadImage.sprite = notepadOpenSprite;
        notepadTabHeaderText.color = originalHeaderColor;
    }

    public void CloseNotepadTab()
    {
        notepadTab.SetActive(false);
        notepadBottomToggle.gameObject.SetActive(false);
        updateNotepadButton.interactable = true;
    }
    
    private void OnNotepadToggleChanged(bool isOn)
    {
        if (isOn)
        {
            if (notepadImage != null && notepadOpenSprite != null)
            {
                notepadImage.sprite = notepadOpenSprite;
                notepadTabHeaderText.color = originalHeaderColor;
                notepadTab.SetActive(true);
            }
        }
        else
        {
            if (notepadImage != null && notepadClosedSprite != null)
            {
                notepadImage.sprite = notepadClosedSprite;
                notepadTabHeaderText.color = inactiveHeaderColor;
                notepadTab.SetActive(false);
            }
        }
    }

    private void NotepadOpen()
    {
        notepadTab.SetActive(true);
        notepadBottomToggle.gameObject.SetActive(true);
        updateNotepadButton.interactable = false;
    }

    private IEnumerator ShutPCDownCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        shutDownAudio.Play();
        yield return new WaitForSeconds(shutDownAudio.clip.length);
        ExitSimulation();
    }
    

    private void OnOKButtonClicked()
    {
        if (shutDownAudio != null && shutDownAudio.clip != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            clickSoundEffectObject.enabled = false;
            shutDownPanelTab.SetActive(false);
            StartCoroutine(ShutPCDownCoroutine());
        }
    }
    
}
