using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackManager : MonoBehaviour
{
    public static CardBackManager Instance;
    
    [Header("Card Back Sprites")]
    [SerializeField] private Sprite[] cardBackSprites; 
    [SerializeField] private string[] cardBackNames; 
    private int currentCardBackIndex = 0;
    private string saveKey = "SelectedCardBack";
    private bool isApplying = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadSelectedCardBack();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    void Start()
    {
        StartCoroutine(ApplyCardBackWithDelay(GetCurrentCardBack()));
    }
    
    void OnEnable()
    {
        StartCoroutine(ApplyCardBackWithDelay(GetCurrentCardBack()));
    }
    
    public Sprite GetCurrentCardBack()
    {
        if (cardBackSprites != null && currentCardBackIndex < cardBackSprites.Length)
            return cardBackSprites[currentCardBackIndex];
        return null;
    }
    
    public void SetCardBack(int index)
    {
        if (index >= 0 && index < cardBackSprites.Length)
        {
            currentCardBackIndex = index;
            SaveSelectedCardBack();
            ApplyCardBackToAllCards();
        }
    }
    
    public int GetCurrentCardBackIndex()
    {
        return currentCardBackIndex;
    }
    
    public Sprite[] GetAllCardBacks()
    {
        return cardBackSprites;
    }
    
    public string[] GetAllCardBackNames()
    {
        return cardBackNames;
    }
    
    void SaveSelectedCardBack()
    {
        PlayerPrefs.SetInt(saveKey, currentCardBackIndex);
        PlayerPrefs.Save();
    }
    
    void LoadSelectedCardBack()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            currentCardBackIndex = PlayerPrefs.GetInt(saveKey);
        }
    }
    
    public void ApplyCardBackToAllCards()
    {
        if (isApplying) return;
        StartCoroutine(ApplyCardBackWithDelay(GetCurrentCardBack()));
    }
    
    IEnumerator ApplyCardBackWithDelay(Sprite currentBack)
    {
        if (currentBack == null) yield break;
        if (isApplying) yield break;
        
        isApplying = true;
        
        yield return new WaitForSeconds(0.15f);
        
        UpdateCardSprite[] allCards = FindObjectsOfType<UpdateCardSprite>();
        foreach (UpdateCardSprite card in allCards)
        {
            if (card != null)
            {
                card.UpdateCardBackSprite(currentBack);
            }
        }
        
        UpdateDeckButtonSprite(currentBack);
        
        isApplying = false;
    }
    
    void UpdateDeckButtonSprite(Sprite currentBack)
    {
        GameObject deckButton = GameObject.FindGameObjectWithTag("Deck");
        if (deckButton != null)
        {
            SpriteRenderer spriteRenderer = deckButton.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && currentBack != null)
            {
                spriteRenderer.sprite = currentBack;
            }
        }
    }
    
    public void UpdateDeckButton()
    {
        UpdateDeckButtonSprite(GetCurrentCardBack());
    }
}