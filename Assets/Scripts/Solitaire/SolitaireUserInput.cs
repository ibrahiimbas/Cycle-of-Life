using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
            CardSelectable s2 = selected.GetComponent<CardSelectable>();
            if (s1 == null || s2 == null) return;

            bool canStack = (s1.value == 1 && s2.value == 0) ||
                            (s1.suit == s2.suit && s1.value == s2.value + 1);

            if (canStack)
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
                bool isEmpty = s2.value == 0;
                bool suitMatch = s1.suit == s2.suit;

                if (s1.value == 1 && isEmpty) return true;
                if (suitMatch && s1.value == s2.value + 1) return true;
                return false;
            }
            else
            {
                if (!HasNoChildren(selected)) return false;

                if (s1.value == s2.value - 1)
                {
                    bool card1Red = IsRedCard(s1.suit);
                    bool card2Red = IsRedCard(s2.suit);
                    return card1Red != card2Red;
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

        int oldRow = s1.row;
        bool wasInDeckPile = s1.inDeckPile;
        bool wasTop = s1.top;

        if (wasInDeckPile)
        {
            solitaire.tripsOnDisplay.Remove(slot1.name);
            RemoveCardFromDeckTrips(slot1.name);

            if (!s2.top && solitaire.bottoms != null && s2.row >= 0 && s2.row < solitaire.bottoms.Length)
            {
                solitaire.bottoms[s2.row].Add(slot1.name);
            }
        }
        else if (wasTop)
        {
            if (solitaire.topPos != null && oldRow < solitaire.topPos.Length)
            {
                CardSelectable topSelectable = solitaire.topPos[oldRow].GetComponent<CardSelectable>();
                if (topSelectable != null)
                {
                    if (s1.value == 1)
                    {
                        topSelectable.value = 0;
                        topSelectable.suit = null;
                    }
                    else
                    {
                        topSelectable.value = s1.value - 1;
                    }
                }
            }

            if (!s2.top && solitaire.bottoms != null && s2.row >= 0 && s2.row < solitaire.bottoms.Length)
            {
                solitaire.bottoms[s2.row].Add(slot1.name);
            }
        }
        else
        {
            if (solitaire.bottoms != null && oldRow >= 0 && oldRow < solitaire.bottoms.Length)
            {
                RemoveCardAndChildrenFromBottom(slot1, oldRow);
            }

            if (!s2.top && solitaire.bottoms != null && s2.row >= 0 && s2.row < solitaire.bottoms.Length)
            {
                AddCardAndChildrenToBottom(slot1, s2.row);
            }
        }

        float yOffset = .65f;
        if (s2.top || (!s1.top && s1.value == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(
            selected.transform.position.x, 
            selected.transform.position.y - yOffset, 
            selected.transform.position.z - .01f);
        
        slot1.transform.SetParent(selected.transform, true);

        if (!wasInDeckPile && !wasTop && oldRow >= 0 && oldRow < solitaire.bottoms.Length)
        {
            RevealUnderCard(oldRow);
        }

        s1.inDeckPile = false;
        s1.row = s2.row;

        if (s2.top)
        {
            if (solitaire.topPos != null && s2.row < solitaire.topPos.Length)
            {
                CardSelectable topSelectable = solitaire.topPos[s2.row].GetComponent<CardSelectable>();
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
        
        SolitaireScoreKeeper scoreKeeper = FindObjectOfType<SolitaireScoreKeeper>();
        if (scoreKeeper != null)
        {
            scoreKeeper.RefreshTopStacks();
        }
    }

    void RemoveCardFromDeckTrips(string cardName)
    {
        if (solitaire.deckTrips == null) return;
        foreach (List<string> trip in solitaire.deckTrips)
        {
            if (trip.Remove(cardName))
                break;
        }
    }

    void RemoveCardAndChildrenFromBottom(GameObject card, int row)
    {
        if (card == null || solitaire.bottoms == null) return;
        if (row < 0 || row >= solitaire.bottoms.Length) return;

        solitaire.bottoms[row].Remove(card.name);

        foreach (Transform child in card.transform)
        {
            if (child.CompareTag("Card"))
            {
                RemoveCardAndChildrenFromBottom(child.gameObject, row);
            }
        }
    }

    void AddCardAndChildrenToBottom(GameObject card, int row)
    {
        if (card == null || solitaire.bottoms == null) return;
        if (row < 0 || row >= solitaire.bottoms.Length) return;

        solitaire.bottoms[row].Add(card.name);

        foreach (Transform child in card.transform)
        {
            if (child.CompareTag("Card"))
            {
                AddCardAndChildrenToBottom(child.gameObject, row);
            }
        }
    }
    
    void RevealUnderCard(int row)
    {
        if (solitaire.bottomPos == null || row < 0 || row >= solitaire.bottomPos.Length) return;

        GameObject columnRoot = solitaire.bottomPos[row];
        if (columnRoot == null) return;

        GameObject lastCard = FindDeepestCard(columnRoot.transform);

        if (lastCard != null)
        {
            CardSelectable cs = lastCard.GetComponent<CardSelectable>();
            if (cs != null && !cs.faceUp)
            {
                cs.faceUp = true;
                UpdateCardSprite sprite = lastCard.GetComponent<UpdateCardSprite>();
                if (sprite != null)
                {
                    sprite.UpdateCardBackSprite(CardBackManager.Instance?.GetCurrentCardBack());
                    
                }
            }
        }
    }
    
    GameObject FindDeepestCard(Transform parent)
    {
        GameObject deepest = null;
        foreach (Transform child in parent)
        {
            if (!child.CompareTag("Card")) continue;
            GameObject candidate = FindDeepestCard(child);
            deepest = candidate != null ? candidate : child.gameObject;
        }
        return deepest;
    }

    GameObject FindCardInBottomColumn(int row, string cardName)
    {
        if (solitaire.bottomPos == null || row >= solitaire.bottomPos.Length) return null;
        GameObject columnRoot = solitaire.bottomPos[row];
        if (columnRoot == null) return null;
        return FindCardInChildren(columnRoot.transform, cardName);
    }

    GameObject FindCardInChildren(Transform parent, string cardName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == cardName && child.CompareTag("Card"))
                return child.gameObject;

            GameObject found = FindCardInChildren(child, cardName);
            if (found != null) return found;
        }
        return null;
    }

    bool Blocked(GameObject selected)
    {
        if (selected == null) return true;
            
        CardSelectable s2 = selected.GetComponent<CardSelectable>();
        if (s2 == null) return true;
            
        if (s2.inDeckPile)
        {
            return !(solitaire.tripsOnDisplay.Count > 0 && s2.name == solitaire.tripsOnDisplay.Last());
        }
        else
        {
            if (solitaire.bottoms != null && s2.row >= 0 && s2.row < solitaire.bottoms.Length && 
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
        return timer < doubleClickTime && clickCount == 2;
    }

    void AutoStack(GameObject selected)
    {
        if (selected == null) return;
            
        CardSelectable sCard = selected.GetComponent<CardSelectable>();
        if (sCard == null) return;
            
        for (int i = 0; i < solitaire.topPos.Length; i++)
        {
            if (solitaire.topPos[i] == null) continue;
                
            CardSelectable stack = solitaire.topPos[i].GetComponent<CardSelectable>();
            if (stack == null) continue;
                
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
                    if (HasNoChildren(selected))
                    {
                        slot1 = selected;
                        GameObject topCardObj = FindTopCardObject(i, stack);
                        if (topCardObj != null)
                        {
                            Stack(topCardObj);
                        }
                        else
                        {
                            Stack(stack.gameObject);
                        }
                        break;
                    }
                }
            }
        }
    }

    GameObject FindTopCardObject(int topRow, CardSelectable topSlot)
    {
        if (topSlot.value == 0) return topSlot.gameObject;

        string cardName = GetCardNameFromSelectable(topSlot);
        GameObject found = FindCardInChildren(solitaire.topPos[topRow].transform, cardName);
        if (found != null) return found;
        return solitaire.topPos[topRow].gameObject;
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
        if (card == null) return true;
        foreach (Transform child in card.transform)
        {
            if (!child.CompareTag("Card")) continue;
            CardSelectable cs = child.GetComponent<CardSelectable>();
            if (cs != null && cs.faceUp) return false;
        }
        return true;
    }
}