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
   
    [Header("Ascii Art List")]
    [SerializeField] private List<AsciiArtData> asciiArtList = new List<AsciiArtData>();
    
    private AsciiArtData currentAsciiArt;

    private void Start()
    {
        AssignButtonsToArts();
        
        if (asciiArtList.Count > 0 && asciiArtList[0] != null)
        {
            ShowAsciiArt(asciiArtList[0]);
        }
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

}

[System.Serializable]
public class AsciiArtData
{
    [Header("Ascii Art Info")] 
    public string asciiArtName;
    public Sprite asciiArtDataImage;
    public RuntimeAnimatorController  asciiAnimation; 
}
