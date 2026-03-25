using UnityEngine;

public class SolitaireUI : MonoBehaviour
{
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
}
