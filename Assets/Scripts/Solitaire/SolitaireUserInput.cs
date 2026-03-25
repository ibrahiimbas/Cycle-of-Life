using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SolitaireUserInput : MonoBehaviour
{
    public GameObject slot1;
    private Solitaire solitaire;
    private float timer;
    private float doubleClickTime = .3f;
    private int clickCount = 0;

    private void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    private void Update()
    {
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
            Vector3 mousePos= Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                // Determine the hit type
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
        if (!selected.GetComponent<CardSelectable>().faceUp) // if the card clicked on is facedown
        {
            if (!Blocked(selected)) // if the card clicked on is not blocked
            {
                // flip it over
                selected.GetComponent<CardSelectable>().faceUp = true;
                slot1 = this.gameObject;
            }
        }
        
        else if (selected.GetComponent<CardSelectable>().inDeckPile)  // if the card clicked on is in the deck pile with trips 
        {
            // if it is not blocked 
            if (!Blocked(selected))
            {
                if (slot1 == selected) // If the same card is clicked twice 
                {
                    if (DoubleClick())
                    {
                        // Attempt auto stack
                        AutoStack(selected); 
                    }
                }

                else
                {
                    slot1 = selected;
                }
            }
        }
            
     // if the card is face up
        // if there is no card currently selected
            // select the card

        if (slot1 == this.gameObject)
        {
                slot1 = selected;
                
        }
            
        // if there is already a card selected (and it is not the same card)
        
        else if (slot1 != selected)
        {
                // if the new card is eligable to stack on the old card
            if (Stackable(selected))
            {
                Stack(selected);
            }
            else
            {
                // select the new card
                slot1 = selected;
            }
        }
        
        else if (slot1 == selected) // If the same card is clicked twice 
        {
            if (DoubleClick())
            {
                // Attempt auto stack
                AutoStack(selected);
            }
        }
    }

    void Top(GameObject selected)
    {
        if (slot1.CompareTag("Card"))
        {
            if (slot1.GetComponent<CardSelectable>().value == 1)
            {
                Stack(selected);
            }
        }
    }

    void Bottom(GameObject selected)
    {
        // if the card is a king and empty slot as a bottom then stack 
        
        if (slot1.CompareTag("Card"))
        {
            if (slot1.GetComponent<CardSelectable>().value == 13)
            {
                Stack(selected);
            }
        }
    }

    bool Stackable(GameObject selected)
    {
        CardSelectable s1 = slot1.GetComponent<CardSelectable>();
        CardSelectable s2 = selected.GetComponent<CardSelectable>();

        if (!s2.inDeckPile)
        {
            if (s2.top) // if in the top pile must stack suited to Ace to King
            {
                if (s1.suit == s2.suit || (s1.value == 1 && s2.suit == null))
                {
                    if (s1.value == s2.value + 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            else // if in the bottom pile must stack alternate colours King to Ace
            {
                if (s1.value == s2.value - 1)
                {
                    bool card1Red = true;
                    bool card2Red = true;

                    if (s1.suit == "C" || s1.suit == "S")
                    {
                        card1Red = false;
                    }

                    if (s2.suit == "C" || s2.suit == "S")
                    {
                        card2Red = false;
                    }

                    if (card1Red == card2Red)
                    {
                        Debug.Log("Not stackable");
                        return false;
                    }
                    else
                    {
                        Debug.Log("Stackable");
                        return true;
                    }
                }
            }
        }

        return false;

    }

    void Stack(GameObject selected)
    {
        // if on top of king or empty bottom stack the cards in place
        // else stack the cards with a negative y offset 
        
        CardSelectable s1 = slot1.GetComponent<CardSelectable>();
        CardSelectable s2 = selected.GetComponent<CardSelectable>();
        float yOffset = .3f;

        if (s2.top || (!s1.top && s1.value == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z-.01f);
        slot1.transform.parent = selected.transform;

        if (s1.inDeckPile) // removes the cards from the top pile to prevent duplicated cards
        {
            solitaire.tripsOnDisplay.Remove(slot1.name);
        }
        else if (s1.top && s2.top && s1.value == 1) // allows movement of cards between top spots
        {
            solitaire.topPos[s1.row].GetComponent<CardSelectable>().value = 0;
            solitaire.topPos[s1.row].GetComponent<CardSelectable>().suit = null;
        }
        else if (s1.top) // keeps track of the current value of the top decks as a card has been removed
        {
            solitaire.topPos[s1.row].GetComponent<CardSelectable>().value = s1.value - 1;
        }
        else  // removes the card string from the appropriate bottom list 
        {
            solitaire.bottoms[s1.row].Remove(slot1.name);
        }

        s1.inDeckPile = false; // You can't add cards to the trips pile so this is always fine 
        s1.row = s2.row;

        if (s2.top) // Moves a card to the top and assings the top's value and suit
        {
            solitaire.topPos[s1.row].GetComponent<CardSelectable>().value = s1.value;
            solitaire.topPos[s1.row].GetComponent<CardSelectable>().suit = s1.suit;
            s1.top = true;
        }
        else
        {
            s1.top = false;
        }
        
        // After completing the move reset slot1 to be essentially null as being null will break the logic 
        slot1 = this.gameObject; 

    }

    bool Blocked(GameObject selected)
    {
        CardSelectable s2 = selected.GetComponent<CardSelectable>();
        if (s2.inDeckPile)
        {
            if (s2.name == solitaire.tripsOnDisplay.Last()) // If it is the last trip it is not blocked
            {
                return false;
            }
            else
            {
                Debug.Log(s2.name + " is blocked by "+solitaire.tripsOnDisplay.Last());
                return true;
            }
        }
        else
        {
            if (s2.name == solitaire.bottoms[s2.row].Last()) // Check if it is the bottom card
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    bool DoubleClick()
    {
        if (timer < doubleClickTime && clickCount == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AutoStack(GameObject selected)
    {
        for (int i = 0; i < solitaire.tops.Length; i++)
        {
            CardSelectable stack = solitaire.topPos[i].GetComponent<CardSelectable>();
            if (selected.GetComponent<CardSelectable>().value == 1) // If it is an Ace
            {
                if (solitaire.topPos[i].GetComponent<CardSelectable>().value == 0) // And the top position is empty
                {
                    slot1 = selected;
                    Stack(stack.gameObject); // Stack the ace up top
                    break;                      // In the first empty position found
                }
            }
            else
            {
                if ((solitaire.topPos[i].GetComponent<CardSelectable>().suit ==
                     slot1.GetComponent<CardSelectable>().suit) &&
                    (solitaire.topPos[i].GetComponent<CardSelectable>().value ==
                     slot1.GetComponent<CardSelectable>().value - 1))
                {
                    // If it is the last card (if it has no children)
                    if (HasNoChildren(slot1))
                    {
                        slot1 = selected;
                        string lastCardName = stack.suit + stack.value.ToString();

                        if (stack.value == 1)
                        {
                            lastCardName = stack.suit + "A";
                        }

                        if (stack.value == 11)
                        {
                            lastCardName = stack.suit + "J";
                        }

                        if (stack.value == 12)
                        {
                            lastCardName = stack.suit + "Q";
                        }

                        if (stack.value == 13)
                        {
                            lastCardName = stack.suit + "K";
                        }

                        GameObject lastCard = GameObject.Find(lastCardName);
                        Stack(lastCard);
                        break;
                    }
                }
            }
        }
    }

    bool HasNoChildren(GameObject card)
    {
        int i = 0;
        foreach (Transform child in card.transform)
        {
            i++;
        }

        if (i == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
