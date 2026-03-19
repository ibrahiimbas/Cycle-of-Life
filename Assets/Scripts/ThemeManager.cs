using UnityEngine;
using System.Collections.Generic;

public class ThemeManager : MonoBehaviour
{
    [Header("Themes")]
    [SerializeField] private List<Theme> availableThemes;
    [SerializeField] private int defaultThemeIndex = 0;
    
    private Theme currentTheme;
    private GameBoard gameBoard;
    private Camera mainCamera;
    
    public delegate void ThemeChangedHandler(Theme newTheme);
    public event ThemeChangedHandler OnThemeChanged;
    
    void Start()
    {
        gameBoard = FindObjectOfType<GameBoard>();
        mainCamera = Camera.main;
        
        if (availableThemes != null && availableThemes.Count > defaultThemeIndex)
        {
            ApplyTheme(availableThemes[defaultThemeIndex]);
        }
    }
    
    public void ApplyTheme(Theme theme)
    {
        if (theme == null) return;
        
        currentTheme = theme;
        
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = theme.backgroundColor;
        }
        
        if (gameBoard != null)
        {
            gameBoard.ApplyTheme(theme);
        }
        
        OnThemeChanged?.Invoke(theme);
        
        Debug.Log($"Theme applied: {theme.name}");
    }
    
    public Theme GetCurrentTheme()
    {
        return currentTheme;
    }
    
    public List<Theme> GetAllThemes()
    {
        return availableThemes;
    }
    
    public void ApplyThemeByIndex(int index)
    {
        if (index >= 0 && index < availableThemes.Count)
        {
            ApplyTheme(availableThemes[index]);
        }
    }
    
    public void ApplyThemeByName(string themeName)
    {
        Theme theme = availableThemes.Find(t => t.name == themeName);
        if (theme != null)
        {
            ApplyTheme(theme);
        }
    }
}