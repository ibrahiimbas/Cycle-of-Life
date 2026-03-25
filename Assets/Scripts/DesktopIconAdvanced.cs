using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class DesktopIconAdvanced : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Settings")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI iconText;
    [SerializeField] private GameObject selectionBorder;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private Color hoverColor = Color.gray;
    
    [Header("Click Settings")]
    [SerializeField] private float doubleClickThreshold = 0.3f;
    [SerializeField] private float sceneLoadDelay = 0.25f;
    [SerializeField] private bool deselectOnDoubleClick = true;
    
    [Header("Action Settings")]
    [SerializeField] private string targetScene;
    [SerializeField] private UnityEngine.Events.UnityEvent onSingleClick;
    [SerializeField] private UnityEngine.Events.UnityEvent onDoubleClick;
    
    private float lastClickTime;
    private bool isSelected;
    private Coroutine singleClickCoroutine;
    private Coroutine doubleClickCoroutine;
    
    private static DesktopIconAdvanced currentlySelected;
    
    private void Start()
    {
        if (selectionBorder != null)
            selectionBorder.SetActive(false);
            
        if (iconImage != null)
            iconImage.color = normalColor;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 1)
        {
            if (singleClickCoroutine != null)
                StopCoroutine(singleClickCoroutine);
            
            singleClickCoroutine = StartCoroutine(SingleClickHandler());
        }
        else if (eventData.clickCount == 2)
        {
            if (singleClickCoroutine != null)
            {
                StopCoroutine(singleClickCoroutine);
                singleClickCoroutine = null;
            }
            
            HandleDoubleClick();
        }
    }
    
    private IEnumerator SingleClickHandler()
    {
        yield return new WaitForSeconds(doubleClickThreshold);
        
        HandleSingleClick();
        singleClickCoroutine = null;
    }
    
    private void HandleSingleClick()
    {
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.Deselect();
        }
        
        isSelected = true;
        currentlySelected = this;
        
        if (selectionBorder != null)
            selectionBorder.SetActive(true);
            
        if (iconImage != null)
            iconImage.color = selectedColor;
        
        onSingleClick?.Invoke();
    }
    
    private IEnumerator DoubleClickHandler()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
    
        if (!string.IsNullOrEmpty(targetScene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
        }
    
        onDoubleClick?.Invoke();
    
        if (deselectOnDoubleClick)
        {
            Deselect();
        }
    
        doubleClickCoroutine = null;
    }
    
    private void HandleDoubleClick()
    {
        if (doubleClickCoroutine != null)
        {
            StopCoroutine(doubleClickCoroutine);
            doubleClickCoroutine = null;
        }
        
        doubleClickCoroutine = StartCoroutine(DoubleClickHandler());
    }
    
    private void Deselect()
    {
        isSelected = false;
        
        if (selectionBorder != null)
            selectionBorder.SetActive(false);
            
        if (iconImage != null)
            iconImage.color = normalColor;
        
        if (currentlySelected == this)
            currentlySelected = null;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected && iconImage != null)
            iconImage.color = hoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected && iconImage != null)
            iconImage.color = normalColor;
    }
    
    public static void ClearSelection()
    {
        if (currentlySelected != null)
        {
            currentlySelected.Deselect();
        }
    }
}