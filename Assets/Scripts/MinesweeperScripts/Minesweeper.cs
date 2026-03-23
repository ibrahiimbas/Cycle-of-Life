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
    private int width;
    private int height;
    private int mineCount;
    private float cameraFov;
    private float cameraOffset;
    
    [Header("Smiley Settings")]
    private Sprite smileyClassic;
    [SerializeField] private Sprite smileyDead;
    [SerializeField] private Sprite smileyCool;
    [SerializeField] private Image buttonSprite;
    [SerializeField] private Button smileyButton;
    
    [Header("Info Texts")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI mineCountText;

    [Header("UI Buttons")] 
    [SerializeField] private Button exitMineButton;
    
    private float timer;
    private bool isTimerRunning;
    
    private int flagCount;
    
    [Header("Ingame Settings")]
    [SerializeField] private Toggle openSettingsToggle;
    [SerializeField] private GameObject gameSettingsPanel;

    [Header("Game Difficulty Settings")] [SerializeField]
    private List<MinesweeperGameSettings> gameDifficulty = new List<MinesweeperGameSettings>();

    [SerializeField] private Toggle beginnerToggle;
    [SerializeField] private Toggle intermediateToggle;
    [SerializeField] private Toggle expertToggle;

    [Header("Audio")] 
    [SerializeField] private AudioSource lostAudio;
    [SerializeField] private AudioSource winAudio;
    [SerializeField] private AudioSource notifyAudio;
    
    private MineBoard board;
    private MineCell[,] state;
    private bool gameOver;
    private Difficulty currentDifficulty;
    private bool isNewGameStarting;
    private bool isSettingUpToggles;
    private bool firstMoveMade;
    private bool isHelpPanelOpen = false;
    
    [Header("Active Inactive Settings")]
    private Color originalHeaderColor;
    [SerializeField] private Color inactiveHeaderColor;
    [SerializeField] private Sprite tabOpenSprite;
    [SerializeField] private Sprite tabClosedSprite;
    [SerializeField] private TextMeshProUGUI headerMainText;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button closeHelpButton;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private Image mainTabImage;
    
    
    private enum Difficulty
    {
        Beginner,
        Intermediate,
        Expert
    }

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
        SetupDifficultyToggles();
        
        currentDifficulty = Difficulty.Intermediate;
        isSettingUpToggles = true;
        intermediateToggle.isOn = true;
        beginnerToggle.isOn = false;
        expertToggle.isOn = false;
        isSettingUpToggles = false;
        
        LoadDifficultySettings(Difficulty.Intermediate);
        NewGame();
    }

    private void Update()
    {
        if (isNewGameStarting) return;
        
        if (isHelpPanelOpen) return;
        
        if (isTimerRunning && !gameOver)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();
        }
        
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
    
    private void SetupDifficultyToggles()
    {
        beginnerToggle.onValueChanged.AddListener((isOn) => {
            if (isOn && !isNewGameStarting)
            {
                intermediateToggle.isOn = false;
                expertToggle.isOn = false;
                ChangeDifficulty(Difficulty.Beginner);
            }
        });
        
        intermediateToggle.onValueChanged.AddListener((isOn) => {
            if (isOn && !isNewGameStarting)
            {
                beginnerToggle.isOn = false;
                expertToggle.isOn = false;
                ChangeDifficulty(Difficulty.Intermediate);
            }
        });
        
        expertToggle.onValueChanged.AddListener((isOn) => {
            if (isOn && !isNewGameStarting)
            {
                beginnerToggle.isOn = false;
                intermediateToggle.isOn = false;
                ChangeDifficulty(Difficulty.Expert);
            }
        });
    }
    
    private void ChangeDifficulty(Difficulty newDifficulty)
    {
        if (currentDifficulty != newDifficulty)
        {
            currentDifficulty = newDifficulty;
            LoadDifficultySettings(newDifficulty);
        }
        
        NewGame();
    }

    private void RestartWithCurrentDifficulty()
    {
        LoadDifficultySettings(currentDifficulty);
        NewGame();
    }
    
     private void LoadDifficultySettings(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Beginner:
                if (gameDifficulty.Count > 0 && gameDifficulty[0] != null)
                {
                    width = gameDifficulty[0].width;
                    height = gameDifficulty[0].height;
                    mineCount = gameDifficulty[0].mineCount;
                    cameraFov = gameDifficulty[0].cameraFov;
                    cameraOffset = gameDifficulty[0].cameraOffset;
                }
                else
                {
                    width = 9;
                    height = 9;
                    mineCount = 10;
                    cameraFov = 10;
                }
                break;
                
            case Difficulty.Intermediate:
                if (gameDifficulty.Count > 1 && gameDifficulty[1] != null)
                {
                    width = gameDifficulty[1].width;
                    height = gameDifficulty[1].height;
                    mineCount = gameDifficulty[1].mineCount;
                    cameraFov = gameDifficulty[1].cameraFov;
                    cameraOffset = gameDifficulty[1].cameraOffset;
                }
                else
                {
                    width = 16;
                    height = 16;
                    mineCount = 40;
                    cameraFov = 10;
                }
                break;
                
            case Difficulty.Expert:
                if (gameDifficulty.Count > 2 && gameDifficulty[2] != null)
                {
                    width = gameDifficulty[2].width;
                    height = gameDifficulty[2].height;
                    mineCount = gameDifficulty[2].mineCount;
                    cameraFov = gameDifficulty[2].cameraFov;
                    cameraOffset = gameDifficulty[2].cameraOffset;
                }
                else
                {
                    width = 30;
                    height = 16;
                    mineCount = 99;
                    cameraFov = 10;
                }
                break;
        }
        
        OnValidate();
    }

    private void StartingSteps()
    {
        buttonSprite.sprite = smileyClassic;
        gameOver = false;
        ResetTimer();
        
        if (openSettingsToggle != null)
        {
            openSettingsToggle.isOn = false;
            gameSettingsPanel.SetActive(false);
        }
    }

    private void ButtonSetups()
    {
        openSettingsToggle.onValueChanged.AddListener(SettingsPanelOnOff);
        smileyButton.onClick.AddListener(RestartWithCurrentDifficulty);
        exitMineButton.onClick.AddListener(GoMainMenu);
        helpButton.onClick.AddListener(OpenHelpPanel);
        closeHelpButton.onClick.AddListener(CloseHelpPanel);
        originalHeaderColor = headerMainText.color;
    }

    private void OpenHelpPanel()
    {
        notifyAudio.Play();
        helpPanel.SetActive(true);
        isHelpPanelOpen = true;
        headerMainText.color = inactiveHeaderColor;
        mainTabImage.sprite = tabClosedSprite;
        helpButton.interactable = false;
        exitMineButton.interactable = false;
        smileyButton.interactable = false;
        openSettingsToggle.interactable = false;
    }

    private void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
        isHelpPanelOpen = false;
        headerMainText.color = originalHeaderColor;
        mainTabImage.sprite = tabOpenSprite;
        helpButton.interactable = true;
        exitMineButton.interactable = true;
        smileyButton.interactable = true;
        openSettingsToggle.interactable = true;
    }

    private void GoMainMenu()
    {
        SceneManager.LoadScene("RulesScene", LoadSceneMode.Single);
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
        StartCoroutine(NewGameCoroutine());
    }
    
    private IEnumerator NewGameCoroutine()
    {
        isNewGameStarting = true;
        
        ResetTimer();
        
        if (board != null && board.tilemap != null)
        {
            board.tilemap.ClearAllTiles();
        }
        
        StartingSteps();
        state = new MineCell[width, height];
        gameOver = false;
        
        GenerateCells();
        GenerateMines();
        GenerateNumbers();
        
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f + cameraOffset, -10f);
        Camera.main.orthographicSize = cameraFov;
        
        flagCount = mineCount;
        if (mineCountText != null)
        {
            mineCountText.text = flagCount.ToString();
        }
        
        if (board != null)
        {
            board.Draw(state);
        }
        
        yield return null;
        isNewGameStarting = false;
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

        StartTimerOnFirstMove();
        
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
        gameOver=true;
        isTimerRunning = false;
        lostAudio.Play();
        
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
        
       
        gameOver=true;
        isTimerRunning = false;
        winAudio.Play();
        
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
    
    private void ResetTimer()
    {
        timer = 0f;
        isTimerRunning = false;
        firstMoveMade = false;
        UpdateTimerDisplay();
    }
    
    private void UpdateTimerDisplay()
    {
        if (timeText != null)
        {
            int seconds = Mathf.FloorToInt(timer);
            timeText.text = seconds.ToString("000");
        }
    }
    
    private void StartTimerOnFirstMove()
    {
        if (!firstMoveMade && !gameOver)
        {
            firstMoveMade = true;
            isTimerRunning = true;
        }
    }
    
}
