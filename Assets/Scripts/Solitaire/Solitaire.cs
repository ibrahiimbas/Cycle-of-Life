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
   
   public GameObject[] bottomPos;
   public GameObject[] topPos;
   
   public static string [] suits = new string[]{"C", "D", "H", "S"};
   public static string[] values = new string[] {"A","2","3","4","5","6","7","8","9","10","J","Q","K" };
   public List<string>[] bottoms;
   public List<string>[] tops;
   
   private List<string> bottom0=new List<string>();
   private List<string> bottom1=new List<string>();
   private List<string> bottom2=new List<string>();
   private List<string> bottom3=new List<string>();
   private List<string> bottom4=new List<string>();
   private List<string> bottom5=new List<string>();
   private List<string> bottom6=new List<string>();

   public List<string> deck;

   private void Start()
   {
      bottoms = new List<string>[]{bottom0,bottom1,bottom2,bottom3,bottom4,bottom5,bottom6};
      PlayCards();
   }

   public void PlayCards()
   {
      deck = GenerateDeck();
      Shuffle(deck);
      
      // For test
      foreach (string card in deck)
      {
         Debug.Log(card);
      }
      
      SolitaireSort();
      StartCoroutine(SolitaireDeal());
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
         foreach (string card in bottoms[i])
         {
            yield return new WaitForSeconds(.01f);
            GameObject newCard = Instantiate(cardPrefab,
               new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset),
               Quaternion.identity, bottomPos[i].transform);
            newCard.name = card;

            if(card==bottoms[i][bottoms[i].Count-1])
            {
               newCard.GetComponent<CardSelectable>().faceUp = true;
            }

            yOffset = yOffset + .6f;
            zOffset = zOffset + .03f;
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
}
