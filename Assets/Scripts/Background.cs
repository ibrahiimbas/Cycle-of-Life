using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


//Currently not using
public class Background : MonoBehaviour
{
    public Texture2D backgroundTexture;
    public int pixelDensity = 10;
    public Color pixelColor=Color.green;

    private void Start()
    {
        CreateBackground();
    }

    private void CreateBackground()
    {
        int width = Screen.width / pixelDensity;
        int height = Screen.height / pixelDensity;

        backgroundTexture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.value>0.5f)
                {
                    backgroundTexture.SetPixel(x,y,pixelColor);
                }
                else
                {
                    backgroundTexture.SetPixel(x,y,Color.black);
                }
            }
            
        }
        backgroundTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = backgroundTexture;
    }

    private void Update()
    {
        CreateBackground();
    }
}
