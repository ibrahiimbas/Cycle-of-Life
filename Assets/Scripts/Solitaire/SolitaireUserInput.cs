using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // List için eklendi

public class SolitaireUserInput : MonoBehaviour
{
    public GameObject slot1;
    private Solitaire solitaire;
    private float timer;
    private float doubleClickTime = .3f;
    private int clickCount = 0;
    private GameObject lastSelectedCard;

    private void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    private void Update()
    {
        if (slot1 != null && slot1 != this.gameObject && slot1.GetComponent<CardSelectable>() == null)
        {
            slot1 = this.gameObject;
        }
        
        if (clickCount == 1)
        {
            timer += Time.deltaTime;    
        }

        if (clickCount == 3)
        {
            timer = 0;
            clickCount = 1;
        }

        if (timer > doubleClickTime)
        {
            timer = 0;
            clickCount = 0;
        }
        
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    Deck();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    Card(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    Top(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    Bottom(hit.collider.gameObject);
                }
            }
        }
    }

    void Deck()
    {
        solitaire.DealFromDeck();
    }

    void Card(GameObject selected)
    {
        if (selected == null)
        {
            slot1 = this.gameObject;
            return;
        }
        
        CardSelectable sCard = selected.GetComponent<CardSelectable>();
        if (sCard == null)
        {
            slot1 = this.gameObject;
            return;
        }
        
        if (sCard.inDeckPile && !IsTopDeckCard(selected))
        {
            return;
        }
        
        if (!sCard.faceUp)
        {
            if (!Blocked(selected))
            {
                sCard.faceUp = true;
                slot1 = this.gameObject;
            }
            return;
        }
        
        if (slot1 == null || slot1 == this.gameObject || slot1.GetComponent<CardSelectable>() == null)
        {
            slot1 = selected;
            lastSelectedCard = selected;
            return;
        }
        
        if (slot1 == selected)
        {
            if (DoubleClick())
            {
                AutoStack(selected);
            }
            return;
        }
        
        if (Stackable(selected))
        {
            Stack(selected);
        }
        else
        {
            slot1 = selected;
            lastSelectedCard = selected;
        }
    }

    bool IsTopDeckCard(GameObject card)
    {
        if (solitaire.tripsOnDisplay.Count > 0)
        {
            string topCardName = solitaire.tripsOnDisplay.Last();
            return card.name == topCardName;
        }
        return false;
    }

    void Top(GameObject selected)
    {
        if (slot1 == null || slot1 == this.gameObject || slot1.GetComponent<CardSelectable>() == null)
            return;
            
        if (slot1.CompareTag("Card"))
        {
            CardSelectable s1 = slot1.GetComponent<CardSelectable>();
            if (s1 != null && s1.value == 1)
            {
                Stack(selected);
            }
        }
    }

    void Bottom(GameObject selected)
    {
        if (slot1 == null || slot1 == this.gameObject || slot1.GetComponent<CardSelectable>() == null)
            return;
            
        if (slot1.CompareTag("Card"))
        {
            CardSelectable s1 = slot1.GetComponent<CardSelectable>();
            if (s1 != null && s1.value == 13)
            {
                Stack(selected);
            }
        }
    }

    bool Stackable(GameObject selected)
    {
        if (slot1 == null || selected == null)
            return false;
            
        CardSelectable s1 = slot1.GetComponent<CardSelectable>();
        CardSelectable s2 = selected.GetComponent<CardSelectable>();
        
        if (s1 == null || s2 == null)
            return false;
        
        if (s2.inDeckPile && !IsTopDeckCard(selected))
        {
            return false;
        }

        if (!s2.inDeckPile)
        {
            if (s2.top)
            {
                if (s1.suit == s2.suit || (s1.value == 1 && s2.suit == null))
                {
                    if (s1.value == s2.value + 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                if (s1.value == s2.value - 1)
                {
                    bool card1Red = IsRedCard(s1.suit);
                    bool card2Red = IsRedCard(s2.suit);

                    if (card1Red == card2Red)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    bool IsRedCard(string suit)
    {
        return suit == "H" || suit == "D";
    }

    void Stack(GameObject selected)
    {
        if (slot1 == null || selected == null)
        {
            slot1 = this.gameObject;
            return;
        }
        
        CardSelectable s1 = slot1.GetComponent<CardSelectable>();
        CardSelectable s2 = selected.GetComponent<CardSelectable>();
        
        if (s1 == null || s2 == null)
        {
            slot1 = this.gameObject;
            return;
        }
        
        float yOffset = .3f;

        if (s2.top || (!s1.top && s1.value == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(
            selected.transform.position.x, 
            selected.transform.position.y - yOffset, 
            selected.transform.position.z - .01f);
        
        slot1.transform.SetParent(selected.transform, true);
        
        if (s1.inDeckPile)
        {
            solitaire.tripsOnDisplay.Remove(slot1.name);
        }
        else if (s1.top && s2.top && s1.value == 1)
        {
            if (solitaire.topPos != null && s1.row < solitaire.topPos.Length)
            {
                CardSelectable topSelectable = solitaire.topPos[s1.row].GetComponent<CardSelectable>();
                if (topSelectable != null)
                {
                    topSelectable.value = 0;
                    topSelectable.suit = null;
                }
            }
        }
        else if (s1.top)
        {
            if (solitaire.topPos != null && s1.row < solitaire.topPos.Length)
            {
                CardSelectable topSelectable = solitaire.topPos[s1.row].GetComponent<CardSelectable>();
                if (topSelectable != null)
                {
                    topSelectable.value = s1.value - 1;
                }
            }
        }
        else
        {
            if (solitaire.bottoms != null && s1.row < solitaire.bottoms.Length)
            {
                solitaire.bottoms[s1.row].Remove(slot1.name);
                RevealUnderCard(s1.row);
            }
        }

        s1.inDeckPile = false;
        s1.row = s2.row;

        if (s2.top)
        {
            if (solitaire.topPos != null && s1.row < solitaire.topPos.Length)
            {
                CardSelectable topSelectable = solitaire.topPos[s1.row].GetComponent<CardSelectable>();
                if (topSelectable != null)
                {
                    topSelectable.value = s1.value;
                    topSelectable.suit = s1.suit;
                }
            }
            s1.top = true;
        }
        else
        {
            s1.top = false;
        }
        
        slot1 = this.gameObject;
        lastSelectedCard = null;
    }
    
    void RevealUnderCard(int row)
    {
        if (solitaire.bottoms != null && row < solitaire.bottoms.Length && solitaire.bottoms[row].Count > 0)
        {
            string lastCardName = solitaire.bottoms[row].Last();
            GameObject lastCard = GameObject.Find(lastCardName);
            if (lastCard != null)
            {
                CardSelectable lastCardSelectable = lastCard.GetComponent<CardSelectable>();
                if (lastCardSelectable != null && !lastCardSelectable.faceUp)
                {
                    lastCardSelectable.faceUp = true;
                }
            }
        }
    }

    bool Blocked(GameObject selected)
    {
        if (selected == null)
            return true;
            
        CardSelectable s2 = selected.GetComponent<CardSelectable>();
        if (s2 == null)
            return true;
            
        if (s2.inDeckPile)
        {
            if (solitaire.tripsOnDisplay.Count > 0 && 
                s2.name == solitaire.tripsOnDisplay.Last())
            {
                return false;
            }
            return true;
        }
        else
        {
            if (solitaire.bottoms != null && s2.row < solitaire.bottoms.Length && 
                solitaire.bottoms[s2.row].Count > 0 &&
                s2.name == solitaire.bottoms[s2.row].Last())
            {
                return false;
            }
            return true;
        }
    }

    bool DoubleClick()
    {
        if (timer < doubleClickTime && clickCount == 2)
        {
            return true;
        }
        return false;
    }

    void AutoStack(GameObject selected)
    {
        if (selected == null)
            return;
            
        CardSelectable sCard = selected.GetComponent<CardSelectable>();
        if (sCard == null)
            return;
            
        for (int i = 0; i < solitaire.topPos.Length; i++)
        {
            if (solitaire.topPos[i] == null)
                continue;
                
            CardSelectable stack = solitaire.topPos[i].GetComponent<CardSelectable>();
            if (stack == null)
                continue;
                
            if (sCard.value == 1)
            {
                if (stack.value == 0)
                {
                    slot1 = selected;
                    Stack(stack.gameObject);
                    break;
                }
            }
            else
            {
                if (stack.suit == sCard.suit && stack.value == sCard.value - 1)
                {
                    if (HasNoChildren(slot1))
                    {
                        slot1 = selected;
                        string lastCardName = GetCardNameFromSelectable(stack);
                        GameObject lastCard = GameObject.Find(lastCardName);
                        if (lastCard != null)
                        {
                            Stack(lastCard);
                            break;
                        }
                    }
                }
            }
        }
    }
    
    string GetCardNameFromSelectable(CardSelectable card)
    {
        string valueString = card.value.ToString();
        if (card.value == 1) valueString = "A";
        if (card.value == 11) valueString = "J";
        if (card.value == 12) valueString = "Q";
        if (card.value == 13) valueString = "K";
        return card.suit + valueString;
    }

    bool HasNoChildren(GameObject card)
    {
        if (card == null)
            return true;
            
        int i = 0;
        foreach (Transform child in card.transform)
        {
            i++;
        }
        return i == 0;
    }
}