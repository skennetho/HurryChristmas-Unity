using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public UnityEvent<int> OnStartGame = new();
    public const string PlayeLayer = "Player";
    public const string ItemTag = "Item";
    private const int INITIAL_LEVEL = 1;
    private static Vector3 PLAYER_DEFAULT_SPAWN_POS = new Vector3(0.0f, -0.1f, 0.0f);

    [SerializeField] private PauseCanvas _pauseCanvas;
    [SerializeField] private GamePlayCanvas _gamePlayCanvas;
    [SerializeField] private ReindeerController _snowSled;
    [SerializeField] private GameObject _raindeerPrefab;
    [SerializeField] private GameArea _gameArea; // scale is the map size;

    [SerializeField] public bool IsPause { get; private set; }
    [SerializeField] public bool IsGameEnd { get; private set; } // == StageEnd
    public int CurrentGameLevel { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public ItemController PlayerItemController { get; private set; }
    public ReindeerController SnowSled => _snowSled;
    public Vector3 GameAreaSize => _gameArea.transform.localScale;
    public Vector3 GameAreaLocation => _gameArea.transform.position;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        StartGame(INITIAL_LEVEL);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPause)
            {
                PauseGame();
            }
            else if (IsPause && !IsGameEnd)
            {
                ResumeGame();
            }
        }
    }

    private void StartGame(int Level)
    {
        Debug.Log($"StartGame Level : {Level}");

        //reset player
        PlayerObject = GameObject.FindGameObjectsWithTag("Player").First();
        PlayerItemController = PlayerObject.GetComponent<ItemController>();
        if (PlayerObject == null)
        {
            Debug.Log("No Player Object Found");
        }
        PlayerObject.transform.position = PLAYER_DEFAULT_SPAWN_POS;
        PlayerObject.transform.rotation = Quaternion.identity;

        IsGameEnd = false;
        CurrentGameLevel = Level;


        // create reindeers and set random position 
        _snowSled.ResetAndCreateRaindeers(_raindeerPrefab, Level+5, Level+3);
        _snowSled.OnFinished.RemoveAllListeners();
        _snowSled.OnFinished.AddListener(EndGameWithCompletionCheck);

        // call resume to make timescale = 1;
        IsPause = true; // to make sure ResumeGame works

        //pause canvas setting
        _pauseCanvas.BackToMainBtn.onClick.RemoveAllListeners();
        _pauseCanvas.BackToMainBtn.onClick.AddListener(BackToMain);
        _pauseCanvas.RestartBtn.onClick.RemoveAllListeners();
        _pauseCanvas.RestartBtn.onClick.AddListener(RestartGame);
        _pauseCanvas.Title.text = "Pause";
        _pauseCanvas.SubTitle.text = "ESC to Resume";
        _pauseCanvas.RestartBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";

        //GamePlayCanvas Setting
        _gamePlayCanvas.UpdateTitleAndSubtitle();

        OnStartGame.Invoke(CurrentGameLevel);
        ResumeGame();
    }

    public void PauseGame()
    {
        if (IsPause == false)
        {
            Time.timeScale = 0;
            IsPause = true;
            _pauseCanvas.gameObject.SetActive(true);
        }
        Debug.Log("PauseGame");
    }

    public void ResumeGame()
    {
        if (IsPause == true)
        {
            Time.timeScale = 1;
            IsPause = false;
            _pauseCanvas.gameObject.SetActive(false);
        }
        Debug.Log("ResumeGame");
    }

    public void EndGameWithCompletionCheck(bool isCompleted)
    {
        PauseGame();
        IsGameEnd = true;
        if (isCompleted)
        {
            _pauseCanvas.Title.text = "GameComplete!";
            _pauseCanvas.SubTitle.text = "GoodJob!";

            _pauseCanvas.RestartBtn.GetComponentInChildren<TextMeshProUGUI>().text = "NextLevel";
            _pauseCanvas.RestartBtn.onClick.RemoveAllListeners();
            _pauseCanvas.RestartBtn.onClick.AddListener(NextStage);
        }
        else
        {
            _pauseCanvas.Title.text = "GameOver...";
            _pauseCanvas.SubTitle.text = "Try Again";

            _pauseCanvas.RestartBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
            _pauseCanvas.RestartBtn.onClick.RemoveAllListeners();
            _pauseCanvas.RestartBtn.onClick.AddListener(RestartGame);
        }
    }

    private void NextStage()
    {
        StartGame(CurrentGameLevel + 1);
    }

    public void BackToMain()
    {
        ResumeGame();
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        EndGameWithCompletionCheck(true);
        StartGame(CurrentGameLevel);
        //생성된 플레이어 삭제 or 위치 수정
        //기록 삭제
        //점수 삭제
    }
}
