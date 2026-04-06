using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform windowRect;
    private Vector2 dragOffset;
    private bool isDraggingEnabled = false;
    
    private Vector2 startPosition;
    private bool startPositionSaved = false;
    
    private RectTransform parentRect;

    private void Start()
    {
        windowRect = GetComponent<RectTransform>();
        parentRect = windowRect.parent as RectTransform;
        
        SaveStartPosition();
        
        DragHandle[] dragHandles = GetComponentsInChildren<DragHandle>();
        foreach (var handle in dragHandles)
        {
            handle.OnDragStarted += EnableDragging;
            handle.OnDragEnded += DisableDragging;
        }
    }
    
    private void OnEnable()
    {
        ResetToStartPosition();
    }

    private void OnDestroy()
    {
        DragHandle[] dragHandles = GetComponentsInChildren<DragHandle>();
        foreach (var handle in dragHandles)
        {
            handle.OnDragStarted -= EnableDragging;
            handle.OnDragEnded -= DisableDragging;
        }
    }
    
    private void SaveStartPosition()
    {
        if (windowRect != null)
        {
            startPosition = windowRect.anchoredPosition;
            startPositionSaved = true;
        }
    }
    
    private void ResetToStartPosition()
    {
        if (windowRect != null && startPositionSaved)
        {
            windowRect.anchoredPosition = startPosition;
        }
    }
    
    private void EnableDragging()
    {
        isDraggingEnabled = true;
    }

    private void DisableDragging()
    {
        isDraggingEnabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggingEnabled) return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        dragOffset = windowRect.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggingEnabled) return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        windowRect.anchoredPosition = localPoint + dragOffset;
    }
}