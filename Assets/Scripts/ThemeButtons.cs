using UnityEngine;
using UnityEngine.UI;

public class ThemeButtons : MonoBehaviour
{
    [Header("Theme Buttons")]
    [SerializeField] private Button[] themeButtons;
    [SerializeField] private int[] themeIndexes;
    
    [Header("Theme Manager")]
    [SerializeField] private ThemeManager themeManager;

    void Start()
    {
        for (int i = 0; i < themeButtons.Length; i++)
        {
            int index = i;
            themeButtons[i].onClick.AddListener(() => ChangeTheme(themeIndexes[index]));
        }
    }

    void ChangeTheme(int index)
    {
        if (themeManager != null)
        {
            themeManager.ApplyThemeByIndex(index);
            Debug.Log($"Theme is changed: : Index {index}");
        }
    }
}