using UnityEngine;
using TMPro;
using System.Collections;

public class TextFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    [SerializeField] private int flickerCount = 3;
    [SerializeField] private float flickerSpeed = 0.5f; 
    [SerializeField] private bool loopForever = false;
    
    private Coroutine flickerCoroutine;
    private TextMeshProUGUI currentText;
    
    public void FlickerLoop(TextMeshProUGUI flickerText)
    {
        if (flickerText == null)
        {
            Debug.LogError("FLicker text is missing!");
            return;
        }
        
        currentText = flickerText;
        loopForever = true;
        
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }
        
        flickerCoroutine = StartCoroutine(FlickerRoutine());
    }
    
    public void StopFlicker()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
        
        loopForever = false;
        
        if (currentText != null)
        {
            currentText.alpha = 1f;
        }
    }
    
    private IEnumerator FlickerRoutine()
    {
        int flickeredCount = 0;
        Color originalColor = currentText.color;
        
        while (loopForever || flickeredCount < flickerCount)
        {
            currentText.alpha = 0f;
            yield return new WaitForSeconds(flickerSpeed);
            
            currentText.alpha = 1f;
            yield return new WaitForSeconds(flickerSpeed);
            
            if (!loopForever)
            {
                flickeredCount++;
            }
        }
        
        currentText.alpha = 1f;
        flickerCoroutine = null;
    }
    
    public void SetFlickerSpeed(float newSpeed)
    {
        flickerSpeed = Mathf.Max(0.1f, newSpeed);
    }
    
    public void SetFlickerCount(int newCount)
    {
        flickerCount = Mathf.Max(1, newCount);
    }
}