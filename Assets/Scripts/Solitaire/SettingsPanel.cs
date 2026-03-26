using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsPanel : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;
    
    [Header("Card Back Selection")]
    [SerializeField] private Transform cardBackContainer;
    [SerializeField] private GameObject cardBackButtonPrefab;
    [SerializeField] private Sprite selectedBorderSprite;
    
    private List<GameObject> cardBackButtons = new List<GameObject>();
    private int selectedIndex = 0;
    private int tempSelectedIndex = 0;
    
    private CardBackManager cardBackManager;
    private SolitaireUI solitaireUI;
    
    void Start()
    {
        cardBackManager = CardBackManager.Instance;
        solitaireUI = FindObjectOfType<SolitaireUI>();
        
        if (okButton != null)
            okButton.onClick.AddListener(ApplyAndClose);
        
        if (cancelButton != null)
            cancelButton.onClick.AddListener(CancelAndClose);
        
        CreateCardBackButtons();
        LoadCurrentSelection();
    }
    
    void CreateCardBackButtons()
    {
        if (cardBackManager == null) return;
        
        Sprite[] cardBacks = cardBackManager.GetAllCardBacks();
        string[] cardBackNames = cardBackManager.GetAllCardBackNames();
        
        for (int i = 0; i < cardBacks.Length; i++)
        {
            int index = i;
            GameObject buttonObj = Instantiate(cardBackButtonPrefab, cardBackContainer);
            
            Image buttonImage = buttonObj.GetComponent<Image>();
            if (buttonImage != null && cardBacks[i] != null)
            {
                buttonImage.sprite = cardBacks[i];
            }
            
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SelectCardBack(index));
            }
            
            cardBackButtons.Add(buttonObj);
        }
    }
    
    void SelectCardBack(int index)
    {
        tempSelectedIndex = index;
        UpdateButtonSelectionVisual();
    }
    
    void UpdateButtonSelectionVisual()
    {
        for (int i = 0; i < cardBackButtons.Count; i++)
        {
            Image borderImage = cardBackButtons[i].GetComponent<Image>();
            if (borderImage != null)
            {
                if (i == tempSelectedIndex)
                {
                    borderImage.color = Color.white;
                }
                else
                {
                    borderImage.color = new Color(0.8036f, 0.8036f, 0.8036f, 1);
                }
            }
        }
    }
    
    void LoadCurrentSelection()
    {
        if (cardBackManager != null)
        {
            selectedIndex = cardBackManager.GetCurrentCardBackIndex();
            tempSelectedIndex = selectedIndex;
            UpdateButtonSelectionVisual();
        }
    }
   
    
    void ApplyAndClose()
    {
        selectedIndex = tempSelectedIndex;
        if (cardBackManager != null)
        {
            cardBackManager.SetCardBack(selectedIndex);
        
            Solitaire solitaire = FindObjectOfType<Solitaire>();
            if (solitaire != null)
            {
                solitaire.UpdateDeckButtonSprite();
            }
        }
    
        CloseSettings();
    }
    
    public void CancelAndClose()
    {
        tempSelectedIndex = selectedIndex;
        UpdateButtonSelectionVisual();
        CloseSettings();
    }
    
    void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            
            Time.timeScale = 1;
            
            if (solitaireUI != null)
            {
                solitaireUI.CloseSettingsMainPanel();
            }
        }
    }
}