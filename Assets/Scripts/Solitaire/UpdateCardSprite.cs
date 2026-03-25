using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCardSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private CardSelectable cardSelectable;
    private Solitaire solitaire;
    private SolitaireUserInput solitaireUserInput;

    private void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire= FindObjectOfType<Solitaire>();
        solitaireUserInput = FindObjectOfType<SolitaireUserInput>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = solitaire.cardSprites[i];
                break;
            }

            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardSelectable=GetComponent<CardSelectable>();
    }

    private void Update()
    {
        if (cardSelectable.faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }

        if (solitaireUserInput.slot1)
        {
            if (name == solitaireUserInput.slot1.name)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = new Color(0.8036f, 0.8036f, 0.8036f,1);
            }
        }
    }
}
