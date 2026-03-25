using System;
using UnityEngine;

public class CardSelectable : MonoBehaviour
{
    public bool top=false;
    public string suit;
    public int value;
    public int row;
    public bool faceUp = false;
    public bool inDeckPile = false;

    private string valueString;

    private void Start()
    {
        if (CompareTag("Card"))
        {
            suit = transform.name[0].ToString();
            valueString = "";
        
            for (int i = 1; i < transform.name.Length; i++)
            {
                char c = transform.name[i];
                valueString = valueString + c.ToString();
            }

            switch (valueString)
            {
                case "A": value = 1; break;
                case "J": value = 11; break;
                case "Q": value = 12; break;
                case "K": value = 13; break;
                default: 
                    if (int.TryParse(valueString, out int val))
                        value = val;
                    break;
            }
        }
    }
}
