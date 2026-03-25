using System;
using UnityEngine;

public class SolitaireScoreKeeper : MonoBehaviour
{
    public CardSelectable[] topStacks;
    
    [SerializeField] private AudioSource winAudio;

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
        winAudio.Play();
        Debug.Log("You won");
        Time.timeScale = 0;
    }
}
