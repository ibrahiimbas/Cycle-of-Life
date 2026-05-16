using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using UnityEditor.Animations;

public class AsciiArt : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image asciiArt;
    [SerializeField] private Animator asciiAnimator;
    [SerializeField] private TextMeshProUGUI asciiArtNameText;
    [SerializeField] private RectTransform imageRectTransform;

    [Header("Ascii Buttons")] 
    [SerializeField] private Button eightBallButton;
    [SerializeField] private Button starButton;
    [SerializeField] private Button danceButton;
    [SerializeField] private Button rollingButton;
    
    [Header("Color Buttons")] 
    [SerializeField] private Button whiteButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button cyanButton;
    [SerializeField] private Button yellowButton;
   
    [Header("Ascii Art List")]
    [SerializeField] private List<AsciiArtData> asciiArtList = new List<AsciiArtData>();
    
    private Color whiteColor = Color.white;
    private Color greenColor = Color.green;
    private Color redColor = Color.red;
    private Color blueColor = Color.blue;
    private Color cyanColor = Color.cyan;
    private Color yellowColor = Color.yellow;
    
    private AsciiArtData currentAsciiArt;

    private void Start()
    {
        AssignButtonsToArts();
        
        if (asciiArtList.Count > 0 && asciiArtList[0] != null)
        {
            ShowAsciiArt(asciiArtList[0]);
        }

        ColorButtonCreator();
    }
    
    private void AssignButtonsToArts()
    {
        if (asciiArtList.Count < 4)
        {
            return;
        }
        
        if (eightBallButton != null && asciiArtList[0] != null)
        {
            int index = 0;
            eightBallButton.onClick.RemoveAllListeners();
            eightBallButton.onClick.AddListener(() =>
            {
                ShowAsciiArt(asciiArtList[index]);
                ResizeAsciiArt(asciiArtList[index]);
            });
        }
        else
        {
            
        }
        
        if (starButton != null && asciiArtList[1] != null)
        {
            int index = 1;
            starButton.onClick.RemoveAllListeners();
            starButton.onClick.AddListener(() =>
            {
                ShowAsciiArt(asciiArtList[index]);
                ResizeAsciiArt(asciiArtList[index]);
            });
        }
        else
        {
            
        }
        
        if (danceButton != null && asciiArtList[2] != null)
        {
            int index = 2;
            danceButton.onClick.RemoveAllListeners();
            danceButton.onClick.AddListener(() =>
            {
                ShowAsciiArt(asciiArtList[index]);
                ResizeAsciiArt(asciiArtList[index]);
            });
        }
        else
        {
            
        }
        
        if (rollingButton != null && asciiArtList[3] != null)
        {
            int index = 3;
            rollingButton.onClick.RemoveAllListeners();
            rollingButton.onClick.AddListener(() =>
            {
                ShowAsciiArt(asciiArtList[index]);
                ResizeAsciiArt(asciiArtList[index]);
            });
        }
        else
        {
            
        }
    }
    
    private void ShowAsciiArt(AsciiArtData artData)
    {
        if (artData == null)
        {
            return;
        }
        
        currentAsciiArt = artData;
        
        if (asciiArtNameText != null && !string.IsNullOrEmpty(artData.asciiArtName))
        {
            asciiArtNameText.text = artData.asciiArtName;
        }
        
        if (asciiArt != null && artData.asciiArtDataImage != null)
        {
            asciiArt.sprite = artData.asciiArtDataImage;
        }
        else if (asciiArt != null && artData.asciiArtDataImage == null)
        {
            Debug.LogWarning($"{artData.asciiArtName} is null");
        }
        
        if (asciiAnimator != null && artData.asciiAnimation != null)
        {
            asciiAnimator.runtimeAnimatorController = artData.asciiAnimation;
        }
        else if (asciiAnimator != null && artData.asciiAnimation == null)
        {
            Debug.LogWarning($"{artData.asciiArtName} is null");
        }
    }

    private void ResizeAsciiArt(AsciiArtData artData)
    {
        if (imageRectTransform == null || artData == null) return;
        
        switch (artData.asciiArtName)
        {
            case "8 Ball":
                imageRectTransform.sizeDelta = new Vector2(504, 504);
                break;
            case "Star":
                imageRectTransform.sizeDelta = new Vector2(504, 504);
                break;
            case "Dance":
                imageRectTransform.sizeDelta = new Vector2(409, 504);
                break;
            case "Rolling Emoticons":
                imageRectTransform.sizeDelta = new Vector2(990, 62);
                break;
            default:
                imageRectTransform.sizeDelta = new Vector2(504, 504);
                break;
        }
    }

    private void ColorButtonCreator()
    {
        whiteButton.onClick.AddListener(() => OnColorButtonClick(whiteColor));
        greenButton.onClick.AddListener(() => OnColorButtonClick(greenColor));
        redButton.onClick.AddListener(() => OnColorButtonClick(redColor));
        blueButton.onClick.AddListener(() => OnColorButtonClick(blueColor));
        cyanButton.onClick.AddListener(() => OnColorButtonClick(cyanColor));
        yellowButton.onClick.AddListener(() => OnColorButtonClick(yellowColor));
    }
    private void OnColorButtonClick(Color color)
    {
       asciiArt.color = color;
       asciiArtNameText.color = color;
    }

    public void ResetColorWhenClosed()
    {
        OnColorButtonClick(whiteColor);
        ShowAsciiArt(asciiArtList[0]);
        ResizeAsciiArt(asciiArtList[0]);
    }
    
}

[System.Serializable]
public class AsciiArtData
{
    [Header("Ascii Art Info")] 
    public string asciiArtName;
    public Sprite asciiArtDataImage;
    public RuntimeAnimatorController  asciiAnimation; 
}
