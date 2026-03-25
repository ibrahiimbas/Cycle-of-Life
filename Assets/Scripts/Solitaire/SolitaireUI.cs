using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class SolitaireUI : MonoBehaviour
{
   [SerializeField] private Button exitButton;

   private void Start()
   {
      exitButton.onClick.AddListener(JumpMainScene);
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
}
