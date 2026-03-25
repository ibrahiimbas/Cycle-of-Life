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
        int total = 0;
        foreach (CardSelectable topstack in topStacks)
        {
            total += topstack.value;
        }

        return total >= 52;
    }

    void Win()
    {
        Debug.Log("You won");
        Time.timeScale = 0;
    }
}
