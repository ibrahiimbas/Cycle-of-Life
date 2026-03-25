using System;
using UnityEngine;

public class SolitaireScoreKeeper : MonoBehaviour
{
    public CardSelectable[] topStacks;

    private void Update()
    {
        if (HasWon())
        {
            Win();
        }
    }

    public bool HasWon()
    {
        int i = 0;
        foreach (CardSelectable topstack in topStacks)
        {
            i += topstack.value;
        }

        if (i >= 52)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Win()
    {
        Debug.Log("Win");
    }
}
