using System;
using System.Text;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class SystemProperties : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI registerInfoText;
    [SerializeField] private TextMeshProUGUI computerInfoText;
    
    [Header("Button")]
    [SerializeField] private Button panelButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button okButton;

    [SerializeField] private GameObject panel;


    private void Start()
    {
        panelButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);
        okButton.onClick.AddListener(ClosePanel);
    }

    public void GetSystemProperties()
    {
    #if UNITY_WEBGL
        if (registerInfoText != null)
            registerInfoText.text = "WindowsUser";

        if (computerInfoText != null)
            computerInfoText.text = "Null...";

    #else
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
    #endif
    }

    private void OpenPanel()
    {
        panel.SetActive(true);
        panelButton.interactable = false;
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
        panelButton.interactable = true;
    }

    public void RefreshSystemProperties()
    {
        GetSystemProperties();
    }
}
