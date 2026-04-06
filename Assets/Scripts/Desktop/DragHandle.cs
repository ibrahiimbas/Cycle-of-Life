using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public System.Action OnDragStarted;
    public System.Action OnDragEnded;
    
    private DraggableWindow parentWindow;
    private bool isDragging = false;

    private void Start()
    {
        parentWindow = GetComponentInParent<DraggableWindow>();
        if (parentWindow == null)
        {
            Debug.LogError($"DragHandle on {gameObject.name} could not find DraggableWindow in parents!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentWindow == null) return;
        
        isDragging = true;
        OnDragStarted?.Invoke();
        parentWindow.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentWindow == null || !isDragging) return;
        parentWindow.OnDrag(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentWindow == null || !isDragging) return;
        
        isDragging = false;
        OnDragEnded?.Invoke();
    }
}