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
   }

   public void ResetScene()
   {
      UpdateCardSprite[] cards = FindObjectsOfType<UpdateCardSprite>();
      foreach (UpdateCardSprite card in cards)
      {
         Destroy(card.gameObject);
      }
      ClearTopValues();
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
      headerMainText.color = inactiveHeaderColor;
      mainTabImage.sprite = tabClosedSprite;
      infoOpenButton.interactable = false;
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
      exitButton.interactable = true;
      restartButton.interactable = true;
      //openSettingsToggle.interactable = true;
   }
}
