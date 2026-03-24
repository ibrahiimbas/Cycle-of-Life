using System.Text;
using TMPro;
using UnityEngine;

public class SystemProperties : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI registerInfoText;
    [SerializeField] private TextMeshProUGUI computerInfoText;


    public void GetSystemProperties()
    {
        if (registerInfoText != null)
        {
            registerInfoText.text = SystemInfo.deviceName;
        }
        else
        {
            Debug.LogWarning("registerInfoText is missing");
        }
        
        if (computerInfoText != null)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine($"{SystemInfo.processorType}");
            sb.AppendLine($"{SystemInfo.graphicsDeviceName}");
            int ramMB = SystemInfo.systemMemorySize;
            float ramGB = ramMB / 1024f;
            sb.AppendLine($"{ramMB} MB ({ramGB:F1} GB)");
            
            computerInfoText.text = sb.ToString();
        }
        else
        {
            Debug.LogWarning("computerInfoText is missing");
        }
    }

    public void RefreshSystemProperties()
    {
        GetSystemProperties();
    }
}
