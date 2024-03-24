using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{ 
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] public Pattern pattern;
    [SerializeField] private float updateInterval = 0.05f;

    [SerializeField] private TextMeshProUGUI populationTxt;
    [SerializeField] private TextMeshProUGUI iterationTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    
    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;
    
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
        SetPattern(pattern);
    }
    
    private void SetPattern(Pattern pattern)
    {
        Clear();

        Vector2Int center = pattern.GetCenter();
        
        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattern.cells[i]-center);
            currentState.SetTile(cell, aliveTile);
            aliveCells.Add(cell);
        }

        population = aliveCells.Count;
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
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        var interval = new WaitForSeconds(updateInterval);
        yield return interval;
        while (enabled)
        {
            UpdateState();

            population = aliveCells.Count;
            iterations++;
            time += updateInterval;
            
            yield return interval;
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
                    cellsToCheck.Add(cell + new Vector3Int(x, y,0));
                }
            }
        }

        foreach (Vector3Int cell in cellsToCheck)
        {
            int neighbours = CountNeighbours(cell);
            bool alive = IsAlive(cell);

            if (!alive && neighbours==3)
            {
                nextState.SetTile(cell,aliveTile);    //become alive
                aliveCells.Add(cell);
            }
            else if (alive && (neighbours<2 || neighbours>3))
            {
                nextState.SetTile(cell,deadTile);     //become dead
                aliveCells.Remove(cell);
            }
            else
            {
                nextState.SetTile(cell, currentState.GetTile(cell));  //staying same
            }
        }

        Tilemap temp = currentState;
        currentState = nextState;
        nextState = temp;
        nextState.ClearAllTiles();
        populationTxt.text = population.ToString();
        iterationTxt.text = iterations.ToString();
        timeTxt.text = time.ToString("F2")+" s";
    }

    private int CountNeighbours(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbour = cell + new Vector3Int(x,y,0);

                if (x==0 && y==0)
                {
                 continue;   
                }
                else if (IsAlive(neighbour))
                {
                    count++;
                }
            }
        }
        
        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell)==aliveTile;
    }

    public void Restart()
    {
        Clear();
        SetPattern(pattern);
    }
}
