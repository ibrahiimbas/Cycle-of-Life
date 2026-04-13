using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Solitaire : MonoBehaviour
{
   [Header("Card Sprites")]
   [SerializeField] public Sprite[] cardSprites;
   [SerializeField] private GameObject cardPrefab;
   
   [Header("Deck Button Sprites")]
   [SerializeField] private Sprite deckButtonNormal;
   [SerializeField] private Sprite deckButtonLow;
   
   public GameObject[] bottomPos;
   public GameObject[] topPos;
   public GameObject deckButton;
   
   public static string [] suits = new string[]{"C", "D", "H", "S"};
   public static string[] values = new string[] {"A","2","3","4","5","6","7","8","9","10","J","Q","K" };
   public List<string>[] bottoms;
   public List<string>[] tops;
   public List<string> tripsOnDisplay = new List<string>();
   public List<List<string>> deckTrips = new List<List<string>>(); 
   
   private List<string> bottom0 = new List<string>();
   private List<string> bottom1 = new List<string>();
   private List<string> bottom2 = new List<string>();
   private List<string> bottom3 = new List<string>();
   private List<string> bottom4 = new List<string>();
   private List<string> bottom5 = new List<string>();
   private List<string> bottom6 = new List<string>();

   public List<string> deck;
   public List<string> discardPile = new List<string>();

   private int deckLocation;
   private int trips;
   private int tripsRemainder;
   
   private void Awake()
   {
      FindReferences();
      
      SolitaireScoreKeeper scoreKeeper = FindObjectOfType<SolitaireScoreKeeper>();
      if (scoreKeeper != null)
      {
         scoreKeeper.RefreshTopStacks();
      }
   }

   private void Start()
   {
      bottoms = new List<string>[]{bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6};
      
      if (deckButton != null && CardBackManager.Instance != null)
      {
         SpriteRenderer deckSprite = deckButton.GetComponent<SpriteRenderer>();
         if (deckSprite != null)
         {
            deckSprite.sprite = CardBackManager.Instance.GetCurrentCardBack();
         }
      }
      
      PlayCards();
   }
   
   private void OnEnable()
   {
      FindReferences();
   }
   
   private void OnDestroy()
   {
      StopAllCoroutines();
   }
   
   private void FindReferences()
   {
      if (bottomPos == null || bottomPos.Length == 0 || bottomPos.Any(pos => pos == null))
      {
         GameObject[] foundBottoms = GameObject.FindGameObjectsWithTag("Bottom");
         if (foundBottoms.Length == 7)
         {
            bottomPos = foundBottoms.OrderBy(go => go.transform.position.x).ToArray();
         }
      }
       
      if (topPos == null || topPos.Length == 0 || topPos.Any(pos => pos == null))
      {
         GameObject[] foundTops = GameObject.FindGameObjectsWithTag("Top");
         if (foundTops.Length == 4)
         {
            topPos = foundTops.OrderBy(go => go.transform.position.x).ToArray();
         }
      }
       
      if (deckButton == null)
      {
         deckButton = GameObject.FindGameObjectWithTag("Deck");
      }
   }

   public void PlayCards()
   {
      StopAllCoroutines();
      FindReferences();

      DestroyAllCardObjects();

      foreach (List<string> list in bottoms)
      {
         list.Clear();
      }

      tripsOnDisplay.Clear();
      deckTrips.Clear();
      discardPile.Clear();
    
      deck = GenerateDeck();
      Shuffle(deck);
    
      SolitaireSort();
      StartCoroutine(SolitaireDeal());
      SortDeckIntoTrips();
   }

   void DestroyAllCardObjects()
   {
      if (deckButton != null)
      {
         List<GameObject> toDestroy = new List<GameObject>();
         foreach (Transform child in deckButton.transform)
         {
            if (child.CompareTag("Card"))
               toDestroy.Add(child.gameObject);
         }
         foreach (GameObject go in toDestroy)
            DestroyImmediate(go);
      }

      if (bottomPos != null)
      {
         foreach (GameObject col in bottomPos)
         {
            if (col == null) continue;
            List<GameObject> toDestroy = new List<GameObject>();
            foreach (Transform child in col.transform)
            {
               if (child.CompareTag("Card"))
                  toDestroy.Add(child.gameObject);
            }
            foreach (GameObject go in toDestroy)
               DestroyImmediate(go);
         }
      }

      if (topPos != null)
      {
         foreach (GameObject slot in topPos)
         {
            if (slot == null) continue;
            List<GameObject> toDestroy = new List<GameObject>();
            foreach (Transform child in slot.transform)
            {
               if (child.CompareTag("Card"))
                  toDestroy.Add(child.gameObject);
            }
            foreach (GameObject go in toDestroy)
               DestroyImmediate(go);
         }
      }

      UpdateCardSprite[] remaining = FindObjectsOfType<UpdateCardSprite>();
      foreach (UpdateCardSprite card in remaining)
      {
         if (card != null)
            DestroyImmediate(card.gameObject);
      }
   }
   
   public static List<string> GenerateDeck()
   {
      List<string> newDeck = new List<string>();
      foreach (string s in suits)
      {
         foreach (string v in values)
         {
            newDeck.Add(s+v);
         }
      }
      return newDeck;
   }

   void Shuffle<T>(IList<T> list)
   {
      Random random = new Random();
      int n = list.Count;

      while (n > 1)
      {
         int k = random.Next(n);
         n--;
         T temp = list[k];
         list[k] = list[n];
         list[n] = temp;
      }
   }

   IEnumerator SolitaireDeal()
   {
      for (int i = 0; i < 7; i++)
      {
         float yOffset = 0;
         float zOffset = .03f;
        
         List<string> bottomCards = new List<string>(bottoms[i]);
        
         foreach (string card in bottomCards)
         {
            yield return new WaitForSeconds(.01f);
            
            if (bottomPos[i] == null) continue;
            
            GameObject newCard = Instantiate(cardPrefab,
               new Vector3(bottomPos[i].transform.position.x, 
                  bottomPos[i].transform.position.y - yOffset, 
                  bottomPos[i].transform.position.z - zOffset),
               Quaternion.identity, bottomPos[i].transform);
            newCard.name = card;
            
            CardSelectable cardSelectable = newCard.GetComponent<CardSelectable>();
            if (cardSelectable != null)
            {
               cardSelectable.row = i;
                
               if (card == bottoms[i][bottoms[i].Count - 1])
               {
                  cardSelectable.faceUp = true;
               }
               else
               {
                  cardSelectable.faceUp = false;
               }
            }

            yOffset += .3f;
            zOffset += .03f;
         }
      }
    
      yield return new WaitForSeconds(0.1f);
      if (CardBackManager.Instance != null)
      {
         CardBackManager.Instance.ApplyCardBackToAllCards();
      }
   }

   void SolitaireSort()
   {
      for (int i = 0; i < 7; i++)
      {
         for (int j = i; j < 7; j++)
         {
            bottoms[j].Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
         }
      }
   }

   public void SortDeckIntoTrips()
   {
      if (deck.Count == 0) return;
      
      UpdateDeckButtonSprite();
    
      trips = deck.Count / 3;
      tripsRemainder = deck.Count % 3;
      deckTrips.Clear();

      int modifier = 0;

      for (int i = 0; i < trips; i++)
      {
         List<string> myTrips = new List<string>();
         for (int j = 0; j < 3; j++)
         {
            if (j + modifier < deck.Count)
            {
               myTrips.Add(deck[j + modifier]);
            }
         }
         deckTrips.Add(myTrips);
         modifier = modifier + 3;
      }

      if (tripsRemainder != 0)
      {
         List<string> myRemainders = new List<string>();
         for (int k = 0; k < tripsRemainder; k++)
         {
            if (deck.Count - tripsRemainder + k < deck.Count)
            {
               myRemainders.Add(deck[deck.Count - tripsRemainder + k]);
            }
         }
         deckTrips.Add(myRemainders);
         trips++;
      }

      deckLocation = 0;
   }

   public void DealFromDeck()
   {
      List<GameObject> cardsToDestroy = new List<GameObject>();
      foreach (Transform child in deckButton.transform)
      {
         if (child.CompareTag("Card"))
         {
            cardsToDestroy.Add(child.gameObject);
         }
      }
    
      foreach (GameObject card in cardsToDestroy)
      {
         if (tripsOnDisplay.Contains(card.name))
         {
            discardPile.Add(card.name);
         }
         Destroy(card);
      }

      tripsOnDisplay.Clear();
    
      if (deckLocation < trips)
      {
         float xOffset = 4.5f;
         float zOffset = -.2f;
         float yOffset = 0f;
         
         List<string> currentTrip = new List<string>(deckTrips[deckLocation]);

         currentTrip = currentTrip.Where(c => deck.Contains(c) || discardPile.Contains(c) || tripsOnDisplay.Contains(c)).ToList();
         currentTrip = currentTrip.Where(c => deck.Contains(c)).ToList();

         for (int i = 0; i < currentTrip.Count; i++)
         {
            string card = currentTrip[i];
            GameObject newTopCard = Instantiate(cardPrefab,
               new Vector3(deckButton.transform.position.x + xOffset, 
                  deckButton.transform.position.y + yOffset,
                  deckButton.transform.position.z + zOffset), 
               Quaternion.identity, deckButton.transform);
            
            xOffset = xOffset + .5f;
            zOffset = zOffset - .2f;
            yOffset = yOffset - .1f;
            
            newTopCard.name = card;
            tripsOnDisplay.Add(card);
            
            CardSelectable cardSelectable = newTopCard.GetComponent<CardSelectable>();
            cardSelectable.faceUp = true;
            cardSelectable.inDeckPile = true;
            
            UpdateCardSprite cardSprite = newTopCard.GetComponent<UpdateCardSprite>();
            if (cardSprite != null && CardBackManager.Instance != null)
            {
               cardSprite.UpdateCardBackSprite(CardBackManager.Instance.GetCurrentCardBack());
            }
         }
         deckLocation++;
      
         UpdateDeckButtonSprite();
      }
      else
      {
         RestackTopDeck();
      }
   }

   void RestackTopDeck()
   {
      List<GameObject> cardsToDestroy = new List<GameObject>();
      foreach (Transform child in deckButton.transform)
      {
         if (child.CompareTag("Card"))
         {
            cardsToDestroy.Add(child.gameObject);
         }
      }
      foreach (GameObject card in cardsToDestroy)
      {
         Destroy(card);
      }
   
      tripsOnDisplay.Clear();
   
      deck.Clear();
      for (int i = discardPile.Count - 1; i >= 0; i--)
      {
         deck.Add(discardPile[i]);
      }
      discardPile.Clear();
   
      deckLocation = 0;
      SortDeckIntoTrips();
    
      UpdateDeckButtonSprite();
   }
   
   public void UpdateDeckButtonSprite()
   {
      if (deckButton == null) return;
    
      SpriteRenderer spriteRenderer = deckButton.GetComponent<SpriteRenderer>();
      if (spriteRenderer == null) return;
      
      if (CardBackManager.Instance != null)
      {
         Sprite currentBack = CardBackManager.Instance.GetCurrentCardBack();
         if (currentBack != null)
         {
            spriteRenderer.sprite = currentBack;
            return;
         }
      }
      
      if (deck.Count > 0 && deck.Count <= 3)
      {
         spriteRenderer.sprite = deckButtonLow;
      }
      else
      {
         spriteRenderer.sprite = deckButtonNormal;
      }
   }
}