using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShutDownTabHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Color hoverColor = Color.white;
    private Color originalColor;

    private void Start()
    {
        if (targetText != null)
        {
            originalColor = targetText.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = originalColor;
        }
    }
}