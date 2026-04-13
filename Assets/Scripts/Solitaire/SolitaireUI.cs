using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using UnityEngine.UI;

public class SolitaireUI : MonoBehaviour
{
   [SerializeField] private Button exitButton;
   [SerializeField] private Button infoOpenButton;
   [SerializeField] private Button infoCloseButton;
   [SerializeField] private GameObject infoPanel;
   [SerializeField] private Button restartButton;
   [SerializeField] private Toggle settingsToggle;
   [SerializeField] private Button openSettingsButton;
   [SerializeField] private GameObject settingsMiniPanel;
   [SerializeField] private GameObject settingsMainPanel;
   [SerializeField] private Button settingsCloseButton;
   
   [Header("Active Inactive Settings")]
   private Color originalHeaderColor;
   [SerializeField] private Color inactiveHeaderColor;
   [SerializeField] private Sprite tabOpenSprite;
   [SerializeField] private Sprite tabClosedSprite;
   [SerializeField] private TextMeshProUGUI headerMainText;
   [SerializeField] private Image mainTabImage;
   
   [Header("Audio")] 
   [SerializeField] private AudioSource notifyAudio;
   
   [Header("References")]
   [SerializeField] private SettingsPanel settingsPanel;
   [SerializeField] private SolitaireScoreKeeper scoreKeeper;
   
   [Header("Scroll Views")]
   [SerializeField] private ScrollRect themeScrollRect;

   private void Start()
   {
      originalHeaderColor = headerMainText.color;
      BindButtonEvents();
   }
   
   private void OnEnable()
   {
      
   }
   
   private void BindButtonEvents()
   {
      if (exitButton != null) exitButton.onClick.RemoveAllListeners();
      if (infoOpenButton != null) infoOpenButton.onClick.RemoveAllListeners();
      if (infoCloseButton != null) infoCloseButton.onClick.RemoveAllListeners();
      if (restartButton != null) restartButton.onClick.RemoveAllListeners();
      if (settingsToggle != null) settingsToggle.onValueChanged.RemoveAllListeners();
      if (openSettingsButton != null) openSettingsButton.onClick.RemoveAllListeners();
      if (settingsCloseButton != null) settingsCloseButton.onClick.RemoveAllListeners();
       
      if (exitButton != null) exitButton.onClick.AddListener(JumpMainScene);
      if (infoOpenButton != null) infoOpenButton.onClick.AddListener(OpenInfoPanel);
      if (infoCloseButton != null) infoCloseButton.onClick.AddListener(CloseInfoPanel);
      if (restartButton != null) restartButton.onClick.AddListener(ResetScene);
      if (settingsToggle != null) settingsToggle.onValueChanged.AddListener(SettingsPanelToggle);
      if (openSettingsButton != null) openSettingsButton.onClick.AddListener(OpenSettingsMainPanel);
      if (settingsCloseButton != null && settingsPanel != null)
         settingsCloseButton.onClick.AddListener(settingsPanel.CancelAndClose);
   }

   public void CloseSettingsMainPanel()
   {
      settingsMainPanel.SetActive(false);
      headerMainText.color = originalHeaderColor;
      mainTabImage.sprite = tabOpenSprite;
      infoOpenButton.interactable = true;
      settingsToggle.interactable = true;
      settingsToggle.isOn = false;
      exitButton.interactable = true;
      restartButton.interactable = true;
   }
   
   private void OpenSettingsMainPanel()
   {
      settingsMainPanel.SetActive(true);
      settingsMiniPanel.SetActive(false);
      headerMainText.color = inactiveHeaderColor;
      mainTabImage.sprite = tabClosedSprite;
      infoOpenButton.interactable = false;
      settingsToggle.interactable = false;
      settingsToggle.isOn = false;
      exitButton.interactable = false;
      restartButton.interactable = false;
      StartCoroutine(ResetScrollRectNextFrame(themeScrollRect));
   }
   
   private IEnumerator ResetScrollRectNextFrame(ScrollRect scrollRect)
   {
      yield return null;
      yield return new WaitForEndOfFrame();
      if (scrollRect != null && scrollRect.horizontal)
      {
         scrollRect.horizontalNormalizedPosition = 0f;
      }
   }

   private void SettingsPanelToggle(bool isOn)
   {
      settingsMiniPanel.SetActive(isOn);
   }
   
   public void ResetScene()
   {
      if (scoreKeeper != null)
      {
         scoreKeeper.ResetWin();
         scoreKeeper.RefreshTopStacks();
      }

      Time.timeScale = 1;
      StopAllCoroutines();

      settingsToggle.isOn = false;
      settingsMiniPanel.SetActive(false);

      ClearTopValues();

      Solitaire solitaire = FindObjectOfType<Solitaire>();
      if (solitaire != null)
      {
         solitaire.PlayCards();
      }
    
      Time.timeScale = 1;
   }

   void ClearTopValues()
   {
      Solitaire solitaire = FindObjectOfType<Solitaire>();
      if (solitaire != null && solitaire.topPos != null)
      {
         foreach (GameObject topSlot in solitaire.topPos)
         {
            if (topSlot == null) continue;
            CardSelectable cs = topSlot.GetComponent<CardSelectable>();
            if (cs != null)
            {
               cs.suit = null;
               cs.value = 0;
            }
         }
         return;
      }

      // Yedek: tag ile bul
      CardSelectable[] selectables = FindObjectsOfType<CardSelectable>();
      foreach (CardSelectable selectable in selectables)
      {
         if (selectable.CompareTag("Top"))
         {
            selectable.suit = null;
            selectable.value = 0;
         }
      }
   }

   private void JumpMainScene()
   {
      Time.timeScale = 1;
      SceneManager.LoadScene("RulesScene", LoadSceneMode.Single);
   }

   private void OpenInfoPanel()
   {
      notifyAudio.Play();
      infoPanel.SetActive(true);
      settingsMiniPanel.SetActive(false);
      headerMainText.color = inactiveHeaderColor;
      mainTabImage.sprite = tabClosedSprite;
      infoOpenButton.interactable = false;
      settingsToggle.interactable = false;
      settingsToggle.isOn = false;
      exitButton.interactable = false;
      restartButton.interactable = false;
   }

   private void CloseInfoPanel()
   {
      infoPanel.SetActive(false);
      headerMainText.color = originalHeaderColor;
      mainTabImage.sprite = tabOpenSprite;
      infoOpenButton.interactable = true;
      settingsToggle.interactable = true;
      settingsToggle.isOn = false;
      exitButton.interactable = true;
      restartButton.interactable = true;
   }
}