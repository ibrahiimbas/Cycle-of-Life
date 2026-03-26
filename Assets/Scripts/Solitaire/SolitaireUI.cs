using System;
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

   private void Start()
   {
      exitButton.onClick.AddListener(JumpMainScene);
      infoOpenButton.onClick.AddListener(OpenInfoPanel);
      infoCloseButton.onClick.AddListener(CloseInfoPanel);
      originalHeaderColor =  headerMainText.color;
      settingsToggle.onValueChanged.AddListener(SettingsPanelToggle);
      openSettingsButton.onClick.AddListener(OpenSettingsMainPanel);
      settingsCloseButton.onClick.AddListener(CloseSettingsMainPanel);
   }

   private void CloseSettingsMainPanel()
   {
      settingsMainPanel.SetActive(false);
      headerMainText.color = originalHeaderColor;
      mainTabImage.sprite = tabOpenSprite;
      infoOpenButton.interactable = true;
      settingsToggle.interactable = true;
      settingsToggle.isOn = false;
      exitButton.interactable = true;
      restartButton.interactable = true;
      //openSettingsToggle.interactable = false;
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
      //openSettingsToggle.interactable = false;
   }

   private void SettingsPanelToggle(bool isOn)
   {
      if (isOn)
      {
         settingsMiniPanel.SetActive(true);
      }
      else
      {
         settingsMiniPanel.SetActive(false);
      }
   }
   
   public void ResetScene()
   {
      UpdateCardSprite[] cards = FindObjectsOfType<UpdateCardSprite>();
      foreach (UpdateCardSprite card in cards)
      {
         Destroy(card.gameObject);
      }
      ClearTopValues();
      settingsToggle.isOn = false;
      settingsMiniPanel.SetActive(false);
      FindObjectOfType<Solitaire>().PlayCards();
   }

   void ClearTopValues()
   {
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
      //openSettingsToggle.interactable = false;
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
      //openSettingsToggle.interactable = true;
   }
}
