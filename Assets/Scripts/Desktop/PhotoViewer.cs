using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class PhotoViewer : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image photoDisplayImage;
    [SerializeField] private TextMeshProUGUI photoCountText; 
    [SerializeField] private TextMeshProUGUI photoNameText; 
    
    [Header("Photo List")]
    [SerializeField] private List<PhotoData> photoList = new List<PhotoData>();
    
    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    
    [Header("Settings")]
    [SerializeField] private bool loopEnabled = true;
    [SerializeField] private int startIndex = 0;
    
    private int currentIndex = 0;
    
    private void Start()
    {
        if (photoList == null || photoList.Count == 0)
        {
            return;
        }
        
        if (photoDisplayImage == null)
        {
            return;
        }
        
        currentIndex = Mathf.Clamp(startIndex, 0, photoList.Count - 1);
        
        ShowCurrentPhoto();
        
        if (nextButton != null)
            nextButton.onClick.AddListener(NextPhoto);
        
        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousPhoto);
    }
    
    public void NextPhoto()
    {
        if (photoList.Count == 0) return;
        
        if (currentIndex + 1 < photoList.Count)
        {
            currentIndex++;
        }
        else if (loopEnabled)
        {
            currentIndex = 0;
        }
        else
        {
            return;
        }
        
        ShowCurrentPhoto();
    }
    
    public void PreviousPhoto()
    {
        if (photoList.Count == 0) return;
        
        if (currentIndex - 1 >= 0)
        {
            currentIndex--;
        }
        else if (loopEnabled)
        {
            currentIndex = photoList.Count - 1;
        }
        else
        {
            return;
        }
        
        ShowCurrentPhoto();
    }
    
    private void ShowCurrentPhoto()
    {
        if (photoDisplayImage != null && photoList != null && photoList.Count > 0 && currentIndex < photoList.Count)
        {
            photoDisplayImage.sprite = photoList[currentIndex].photoSprite;
            
            if (photoNameText != null)
            {
                photoNameText.text = photoList[currentIndex].photoName;
            }
            
            UpdatePhotoCountText();
        }
    }
    
    private void UpdatePhotoCountText()
    {
        if (photoCountText != null)
        {
            photoCountText.text = (currentIndex + 1).ToString() + " / " + photoList.Count.ToString();
        }
    }
    
    public void GoToFirstPhoto(int index)
    {
        if (photoList.Count == 0) return;
        
        currentIndex = Mathf.Clamp(index, 0, photoList.Count - 1);
        ShowCurrentPhoto();
    }
    
    private void OnDestroy()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextPhoto);
        
        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousPhoto);
    }
    
}

[System.Serializable]
public class PhotoData
{
    [Header("Photo Info")]
    public string photoName;
    public Sprite photoSprite;
    
    public PhotoData(string name, Sprite sprite)
    {
        photoName = name;
        photoSprite = sprite;
    }
    
    public PhotoData()
    {
        photoName = "New Photo";
        photoSprite = null;
    }
}