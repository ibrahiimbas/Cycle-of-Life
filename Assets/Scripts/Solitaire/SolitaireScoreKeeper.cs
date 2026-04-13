using System;
using System.Linq;
using UnityEngine;

public class SolitaireScoreKeeper : MonoBehaviour
{
    public CardSelectable[] topStacks;
    
    [SerializeField] private AudioSource winAudio;

    private bool hasWon = false;
    private Solitaire solitaire;
    
    private void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        RefreshTopStacks();
    }
    
    private void Update()
    {
        if (!hasWon && HasWon())
        {
            hasWon = true;
            Win();
        }
    }
    
    public void RefreshTopStacks()
    {
        if (solitaire != null && solitaire.topPos != null)
        {
            topStacks = new CardSelectable[solitaire.topPos.Length];
            for (int i = 0; i < solitaire.topPos.Length; i++)
            {
                if (solitaire.topPos[i] != null)
                {
                    topStacks[i] = solitaire.topPos[i].GetComponent<CardSelectable>();
                    if (topStacks[i] == null)
                    {
                        topStacks[i] = solitaire.topPos[i].AddComponent<CardSelectable>();
                        topStacks[i].top = true;
                    }
                }
            }
        }
    }
    
    public void ResetWin() 
    { 
        hasWon = false;
        RefreshTopStacks();
    }

    public bool HasWon()
    {
        RefreshTopStacks();
        
        int total = 0;
        foreach (CardSelectable topstack in topStacks)
        {
            if (topstack != null)
            {
                total += topstack.value;
            }
        }

        return total >= 52;
    }

    void Win()
    {
        if (winAudio != null)
            winAudio.Play();
        Debug.Log("You won!");
        Time.timeScale = 0;
    }
}