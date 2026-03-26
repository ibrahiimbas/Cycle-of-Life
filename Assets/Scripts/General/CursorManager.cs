using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    public Texture2D mainCursor;
    public Texture2D hoverCursor;
    public Texture2D actionCursor;
    
    [Header("Settings")]
    public Vector2 hotSpot = Vector2.zero;
    public float actionDuration = .75f;
    
    private bool isActionActive = false;
    private GameObject lastHoveredButton = null;
    
    void Start()
    {
        SetMainCursor();
    }
    
    void Update()
    {
        GameObject currentButton = GetCurrentButton();
        
        if (currentButton != null)
        {
            if (lastHoveredButton != currentButton)
            {
                SetHoverCursor();
                lastHoveredButton = currentButton;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(ActionCursorRoutine());
            }
        }
        else
        {
            if (lastHoveredButton != null)
            {
                if (!isActionActive)
                {
                    SetMainCursor();
                }
                lastHoveredButton = null;
            }
        }
    }
    
    GameObject GetCurrentButton()
    {
        if (EventSystem.current == null) return null;
        
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);
        
        foreach (var result in raycastResults)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null && button.interactable)
            {
                return result.gameObject;
            }
        }
        
        return null;
    }
    
    void SetMainCursor()
    {
        if (!isActionActive)
        {
            Cursor.SetCursor(mainCursor, hotSpot, CursorMode.Auto);
        }
    }
    
    void SetHoverCursor()
    {
        if (!isActionActive)
        {
            Cursor.SetCursor(hoverCursor, hotSpot, CursorMode.Auto);
        }
    }
    
    IEnumerator ActionCursorRoutine()
    {
        isActionActive = true;
        Cursor.SetCursor(actionCursor, hotSpot, CursorMode.Auto);
        
        yield return new WaitForSeconds(actionDuration);
        
        Cursor.SetCursor(mainCursor, hotSpot, CursorMode.Auto);
        isActionActive = false;
    }
    
    public void TriggerActionCursor()
    {
        StartCoroutine(ActionCursorRoutine());
    }
}
