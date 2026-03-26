using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{ 
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    
    [Header("Tile Theme Settings")]
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] private Theme defaultTheme;
    private Theme currentTheme;
    [SerializeField] private ThemeManager themeManager;
    
    [SerializeField] public Pattern pattern;
    [SerializeField] private float updateInterval = 0.05f;

    [SerializeField] private TextMeshProUGUI populationTxt;
    [SerializeField] private TextMeshProUGUI iterationTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    
    // Beta feature
    [Header("Mouse Interaction Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool enableMouseEditing = false;
    [SerializeField] private KeyCode editModifierKey = KeyCode.LeftControl;
    [SerializeField] private SimulationSettings simulationSettings;
    
    private bool isMouseDragging = false;
    private Vector3Int lastEditedCell;
    
    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;
    private Coroutine simulationCoroutine;
    
    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }

    private void Awake()
    {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }

    private void Start()
    {
        if (themeManager != null)
        {
            themeManager.OnThemeChanged += ApplyTheme;
            Theme initialTheme = themeManager.GetCurrentTheme();
            if (initialTheme != null)
            {
                ApplyTheme(initialTheme);
            }
            else
            {
                currentTheme = defaultTheme;
            }
        }
        else
        {
            currentTheme = defaultTheme;
        }
        
        SetPattern(pattern);

        enableMouseEditing = simulationSettings.isToggleOn;
    }
    
    // Click to add alive cell during game is a beta feature not working properly
    private void Update()
    {
        enableMouseEditing = simulationSettings.isToggleOn;
        if (!enableMouseEditing || simulationSettings.isPaused) return;
        
        HandleMouseInput();
    }

    private void OnDestroy()
    {
        if (themeManager != null)
        {
            themeManager.OnThemeChanged -= ApplyTheme;
        }
    }

    public void ChangeMouseSettings(bool isOn)
    {
        enableMouseEditing = isOn;
    }

    private void SetPattern(Pattern pattern)
    {
        StopSimulation();
        
        Clear();

        Vector2Int center = pattern.GetCenter();
        
        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
            if (currentTheme != null)
            {
                currentState.SetTile(cell, currentTheme.aliveVisual);
            }
            else if (defaultTheme != null)
            {
                currentState.SetTile(cell, defaultTheme.aliveVisual);
            }
            else
            {
                currentState.SetTile(cell, aliveTile);
            }
            
            aliveCells.Add(cell);
        }

        population = aliveCells.Count;
        iterations = 0;
        time = 0f;
        
        UpdateUI();
        
        StartSimulation();
    }

    private void Clear()
    {
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();
        aliveCells.Clear();
        cellsToCheck.Clear();
        population = 0;
        iterations = 0;
        time = 0f;
        isMouseDragging = false;
    }

    private void OnEnable()
    {
        StartSimulation();
    }

    private void OnDisable()
    {
        StopSimulation();
    }

    private void StartSimulation()
    {
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
        }
        simulationCoroutine = StartCoroutine(Simulate());
    }

    private void StopSimulation()
    {
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
            simulationCoroutine = null;
        }
    }

    private IEnumerator Simulate()
    {
        var interval = new WaitForSeconds(updateInterval);
        
        while (enabled)
        {
            yield return interval;
            
            UpdateState();

            population = aliveCells.Count;
            iterations++;
            time += updateInterval;
            
            UpdateUI();
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();
        
        foreach (Vector3Int cell in aliveCells)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    cellsToCheck.Add(cell + new Vector3Int(x, y, 0));
                }
            }
        }

        foreach (Vector3Int cell in cellsToCheck)
        {
            int neighbours = CountNeighbours(cell);
            bool alive = IsAlive(cell);

            if (!alive && neighbours == 3)
            {
                Tile tileToUse = GetAliveTileForCurrentTheme();
                nextState.SetTile(cell, tileToUse);
                aliveCells.Add(cell);
            }
            else if (alive && (neighbours < 2 || neighbours > 3))
            {
                Tile tileToUse = GetDeadTileForCurrentTheme();
                nextState.SetTile(cell, tileToUse);
                aliveCells.Remove(cell);
            }
            else
            {
                nextState.SetTile(cell, currentState.GetTile(cell));
            }
        }

        // Swap Tilemaps
        Tilemap temp = currentState;
        currentState = nextState;
        nextState = temp;
        nextState.ClearAllTiles();
    }

    private Tile GetAliveTileForCurrentTheme()
    {
        if (currentTheme != null && currentTheme.aliveVisual != null)
            return currentTheme.aliveVisual;
        if (defaultTheme != null && defaultTheme.aliveVisual != null)
            return defaultTheme.aliveVisual;
        return aliveTile;
    }

    private Tile GetDeadTileForCurrentTheme()
    {
        if (currentTheme != null && currentTheme.deadVisual != null)
            return currentTheme.deadVisual;
        if (defaultTheme != null && defaultTheme.deadVisual != null)
            return defaultTheme.deadVisual;
        return deadTile;
    }

    private int CountNeighbours(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                
                Vector3Int neighbour = cell + new Vector3Int(x, y, 0);
                if (IsAlive(neighbour))
                {
                    count++;
                }
            }
        }
        
        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        Tile cellTile = currentState.GetTile<Tile>(cell);
        
        if (cellTile == null) return false;
        
        if (currentTheme != null && currentTheme.aliveVisual != null)
            return cellTile == currentTheme.aliveVisual;
        
        if (defaultTheme != null && defaultTheme.aliveVisual != null)
            return cellTile == defaultTheme.aliveVisual;
        
        return cellTile == aliveTile;
    }

    private void UpdateUI()
    {
        if (populationTxt != null)
            populationTxt.text = population.ToString();
        if (iterationTxt != null)
            iterationTxt.text = iterations.ToString();
        if (timeTxt != null)
            timeTxt.text = time.ToString("F2") + " s";
    }

    public void Restart()
    {
        StopSimulation();
        SetPattern(pattern);
        StartSimulation();
    }
    
    public void ApplyTheme(Theme newTheme)
    {
        if (newTheme == null) return;
        
        currentTheme = newTheme;
        
        if (Camera.main != null)
        {
            Camera.main.backgroundColor = newTheme.backgroundColor;
        }
        
        UpdateAllTilesWithNewTheme();
    }
    
    private void UpdateAllTilesWithNewTheme()
    {
        if (currentTheme == null) return;
        
        foreach (Vector3Int cell in aliveCells)
        {
            currentState.SetTile(cell, currentTheme.aliveVisual);
        }
        
        BoundsInt bounds = currentState.cellBounds;
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase currentTile = currentState.GetTile(cellPosition);
            
                if (currentTile != null && !aliveCells.Contains(cellPosition))
                {
                    currentState.SetTile(cellPosition, currentTheme.deadVisual);
                }
            }
        }
        
    }
    
    private void HandleMouseInput()
    {
        //if (IsPointerOverUI()) return;
        
        bool editingEnabled = editModifierKey == KeyCode.None || Input.GetKey(editModifierKey);
        
        if (!editingEnabled) return;
        
        // Mouse position to grid position
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = currentState.WorldToCell(mouseWorldPos);
        
        // If you add one cell during game it suddenly dies because there are no enough neighbours 
        // Dragging is more efficient 
        
        if (Input.GetMouseButtonDown(0))
        {
            AddCell(cellPosition);
            lastEditedCell = cellPosition;
            isMouseDragging = true;
        }
        
        // Hold left click and drag
        if (Input.GetMouseButton(0) && isMouseDragging && cellPosition != lastEditedCell)
        {
            AddCell(cellPosition);
            lastEditedCell = cellPosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }
        
        // Right click removing the cells
        if (Input.GetMouseButtonDown(1))
        {
            RemoveCell(cellPosition);
            lastEditedCell = cellPosition;
            isMouseDragging = true;
        }
        
        if (Input.GetMouseButton(1) && isMouseDragging && cellPosition != lastEditedCell)
        {
            RemoveCell(cellPosition);
            lastEditedCell = cellPosition;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            isMouseDragging = false;
        }
    }
    
    private void AddCell(Vector3Int cellPosition)
    {
        // If this grid position has live cell don't do anything
        if (aliveCells.Contains(cellPosition)) return;
        
        // Stop simulation while adding cell to prevent bugs
        bool wasSimulating = simulationCoroutine != null;
        if (wasSimulating)
        {
            //StopSimulation();
        }
        
        // Add cell
        Tile tileToUse = GetAliveTileForCurrentTheme();
        currentState.SetTile(cellPosition, tileToUse);
        aliveCells.Add(cellPosition);
        
        population = aliveCells.Count;
        UpdateUI();
        
        if (wasSimulating)
        {
            //StartSimulation();
        }
    }
    
    private void RemoveCell(Vector3Int cellPosition)
    {
        if (!aliveCells.Contains(cellPosition)) return;
        
        bool wasSimulating = simulationCoroutine != null;
        if (wasSimulating)
        {
            //StopSimulation();
        }
        
        Tile tileToUse = GetDeadTileForCurrentTheme();
        currentState.SetTile(cellPosition, tileToUse);
        aliveCells.Remove(cellPosition);
        
        population = aliveCells.Count;
        UpdateUI();
        
        if (wasSimulating)
        {
            //StartSimulation();
        }
    }

    
    // Buggy, not working 
    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
    
}