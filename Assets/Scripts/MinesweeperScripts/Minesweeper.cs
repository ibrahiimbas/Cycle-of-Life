using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int width = 16;
    [SerializeField] private int height = 16;
    [SerializeField] private int mineCount = 32;
    [SerializeField] private float cameraFov = 10;
    
    [Header("Smiley Settings")]
    private Sprite smileyClassic;
    [SerializeField] private Sprite smileyDead;
    [SerializeField] private Sprite smileyCool;
    [SerializeField] private Image buttonSprite;
    
    [Header("Info Texts")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI mineCountText;
    private int flagCount;
    
    [Header("Ingame Settings")]
    [SerializeField] private Toggle openSettingsToggle;
    [SerializeField] private GameObject gameSettingsPanel;
    
    private MineBoard board;
    private MineCell[,] state;
    private bool gameOver;

    private void OnValidate()
    {
        mineCount = Mathf.Clamp(mineCount, 0, width*height);
    }

    private void Awake()
    {
        board = GetComponentInChildren<MineBoard>();
    }

    private void Start()
    {
        smileyClassic= buttonSprite.sprite;
        ButtonSetups();
        NewGame();
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Flag();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Reveal();
            }
        }
    }

    private void StartingSteps()
    {
        buttonSprite.sprite = smileyClassic;
        openSettingsToggle.isOn = false;
        gameSettingsPanel.SetActive(false);
    }

    private void ButtonSetups()
    {
        openSettingsToggle.onValueChanged.AddListener(SettingsPanelOnOff);
    }

    private void SettingsPanelOnOff(bool isOn)
    {
        if (isOn)
        {
            gameSettingsPanel.SetActive(true);
        }

        else
        {
            gameSettingsPanel.SetActive(false);
        }
    }

    private void NewGame()
    {
        StartingSteps();
        state = new MineCell[width, height];
        gameOver = false;
        
        GenerateCells();
        GenerateMines();
        GenerateNumbers();
        
        Camera.main.transform.position = new Vector3(width / 2, height / 2+1, -10f);
        flagCount = mineCount;
        mineCountText.text=flagCount.ToString();
        board.Draw(state);
        
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MineCell cell = new MineCell();
                cell.position=new Vector3Int(x,y,0);
                cell.type = MineCell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0,width);
            int y = Random.Range(0,height);

            while (state[x, y].type == MineCell.Type.Mine)
            {
                x++;

                if (x >= width)
                {
                    x = 0;
                    y++;

                    if (y >= height)
                    {
                        y = 0;
                    }
                }
            }
            
            state[x,y].type = MineCell.Type.Mine;
           // state[x,y].revealed = true;
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MineCell cell = state[x, y];

                if (cell.type == MineCell.Type.Mine)
                {
                    continue;
                }

                cell.number = CountMines(x, y);

                if (cell.number > 0)
                {
                    cell.type = MineCell.Type.Number;
                }
                
                //cell.revealed = true;
                state[x, y] = cell;
            }
        }
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0)
                {
                    continue;
                }
                
                int x = cellX+adjacentX;
                int y = cellY+adjacentY;
                
                if (GetCell(x,y).type == MineCell.Type.Mine)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private void Flag()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        MineCell cell = GetCell(cellPos.x, cellPos.y);

        if (cell.type == MineCell.Type.Invalid || cell.revealed)
        {
            return;
        }

        cell.flagged = !cell.flagged;
        if (cell.flagged)
        {
            flagCount--;
            mineCountText.text=flagCount.ToString();
        }
        else
        {
            flagCount++;
            mineCountText.text=flagCount.ToString();
        }
        
        state[cellPos.x, cellPos.y] = cell;
        board.Draw(state);
        
    }

    private void Reveal()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        MineCell cell = GetCell(cellPos.x, cellPos.y);

        if (cell.type == MineCell.Type.Invalid || cell.revealed || cell.flagged)
        {
            return;
        }

        switch (cell.type)
        {
            case MineCell.Type.Mine:
                Explode(cell);
                break;
            case MineCell.Type.Empty:
                Flood(cell);
                CheckWin();
                break;
            default:
                cell.revealed = true;
                state[cellPos.x, cellPos.y] = cell;
                CheckWin();
                break;
        }
        
        board.Draw(state);
    }

    private void Flood(MineCell cell)
    {
        if (cell.revealed) return;
        if (cell.type == MineCell.Type.Mine || cell.type == MineCell.Type.Invalid) return;
        
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == MineCell.Type.Empty)
        {
            // Check every neighbour cell for if there are any empty cell
            Flood(GetCell(cell.position.x-1, cell.position.y));
            Flood(GetCell(cell.position.x+1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y-1));
            Flood(GetCell(cell.position.x, cell.position.y+1));
        }
    }

    private void Explode(MineCell cell)
    {
        Debug.Log("Game Over");
        gameOver=true;
        
        cell.revealed = true;
        cell.exploded=true;
        state[cell.position.x, cell.position.y] = cell;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell =state[x, y];

                if (cell.type == MineCell.Type.Mine)
                {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
        
        buttonSprite.sprite = smileyDead;
    }

    private void CheckWin()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MineCell cell = state[x, y];

                if (cell.type != MineCell.Type.Mine && !cell.revealed)
                {
                    return;
                }
            }   
        }
        
        Debug.unityLogger.Log("You Won");
        gameOver=true;
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MineCell cell =state[x, y];

                if (cell.type == MineCell.Type.Mine)
                {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }
        
        buttonSprite.sprite = smileyCool;
    }

    private MineCell GetCell(int x, int y)
    {
        if (isValid(x, y))
        {
            return state[x, y];
        }
        else
        {
            return new MineCell();
        }
    }

    private bool isValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    
}
