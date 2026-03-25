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
   
   private List<string> bottom0=new List<string>();
   private List<string> bottom1=new List<string>();
   private List<string> bottom2=new List<string>();
   private List<string> bottom3=new List<string>();
   private List<string> bottom4=new List<string>();
   private List<string> bottom5=new List<string>();
   private List<string> bottom6=new List<string>();

   public List<string> deck;
   public List<string> discardPile = new List<string>();

   private int deckLocation;
   private int trips;
   private int tripsRemainder;

   private void Start()
   {
      bottoms = new List<string>[]{bottom0,bottom1,bottom2,bottom3,bottom4,bottom5,bottom6};
      PlayCards();
   }

   public void PlayCards()
   {
      foreach (List<string> list in bottoms)
      {
         list.Clear();
      }
      
      deck = GenerateDeck();
      Shuffle(deck);
      
      // For test
      //foreach (string card in deck)
      //{
      //   Debug.Log(card);
      //}
      
      SolitaireSort();
      StartCoroutine(SolitaireDeal());
      SortDeckIntoTrips();
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
      discardPile.Clear();
    
      for (int i = 0; i < 7; i++)
      {
         float yOffset = 0;
         float zOffset = .03f;
         foreach (string card in bottoms[i])
         {
            yield return new WaitForSeconds(.01f);
            GameObject newCard = Instantiate(cardPrefab,
               new Vector3(bottomPos[i].transform.position.x, 
                  bottomPos[i].transform.position.y - yOffset, 
                  bottomPos[i].transform.position.z - zOffset),
               Quaternion.identity, bottomPos[i].transform);
            newCard.name = card;
            newCard.GetComponent<CardSelectable>().row = i;

            if (card == bottoms[i][bottoms[i].Count - 1])
            {
               newCard.GetComponent<CardSelectable>().faceUp = true;
            }

            yOffset += .3f;
            zOffset += .03f;
            
         }
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
         if (deck.Contains(card.name))
         {
            deck.Remove(card.name);
         }
         discardPile.Add(card.name);
         Destroy(card);
      }
    
      if (deckLocation < trips)
      {
         tripsOnDisplay.Clear();
         float xOffset = 4.5f;
         float zOffset = -.2f;
         float yOffset = 0f;

         for (int i = 0; i < deckTrips[deckLocation].Count; i++)
         {
            string card = deckTrips[deckLocation][i];
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
      deck.Clear();
      foreach (string card in discardPile)
      {
         deck.Add(card);
      }
      discardPile.Clear();
      SortDeckIntoTrips();
   }
   
   public void UpdateDeckButtonSprite()
   {
      if (deckButton == null) return;
    
      SpriteRenderer spriteRenderer = deckButton.GetComponent<SpriteRenderer>();
      if (spriteRenderer == null) return;
    
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
