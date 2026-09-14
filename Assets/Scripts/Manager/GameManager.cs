using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Made by Matthew Chang
public class GameManager : MonoBehaviour
{
    public HighScores highScores;
    //Reference to overlay Text to display winning text, etc
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public GameObject highScorePanel;
    public TextMeshProUGUI highScoresText;

    public Button newGameButton;
    public Button highScoresButton;

    public TargetHealth[] targets;
    public GameObject player;
    public Camera worldCamera;

    public float startTImerAmount = 3;
    private float startTimer;

    public float targetActivateTimerAmount = 1;
    private float targetActivateTimer;

    public float gamerTimerAmount = 60;
    private float gamerTimer;

    private int score = 0;

    public GameObject resetButton; // drag your button GameObject here in Inspector

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    public void OnNewGame()
    {
        gameState = GameState.Start;
    }

    public void OnHighScores()
    {
        messageText.text = "";

        highScoresButton.gameObject.SetActive(false);
        highScorePanel.gameObject.SetActive(true);

        string text = "";
        for (int i = 0; i < highScores.scores.Length; i++)
        {
            text += highScores.scores[i] + "\n";
        }
        highScoresText.text = text;
    }
    private GameState gameState;
    public GameState State { get { return gameState; } }

    private void Awake()
    {
        gameState = GameState.GameOver;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GameManager = this;
            targets[i].gameObject.SetActive(false);
        }

        startTimer = startTImerAmount;
        messageText.text = "Press Enter to Start";
        timerText.text = "";
        scoreText.text = "";

        highScorePanel.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(true);
        highScoresButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch(gameState)
        {
            case GameState.Start:
                GameStateStart();
                break;

            case GameState.Playing:
                GameStatePlaying();
                break;

            case GameState.GameOver:
                GameStateGameOver();
                break;
        }
    }
    //Ramdonly activates a target
    private void ActivateRandomTarget()
    {
        int randomIndex = Random.Range(0, targets.Length);
        targets[randomIndex].gameObject.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }
    public void ResetGame()
    {
        // Stop and hide gameplay
        player.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].gameObject.SetActive(false);
        }

        // Reset stats and UI
        score = 0;
        scoreText.text = "";
        timerText.text = "";
        startTimer = startTImerAmount;

        gameState = GameState.Start;
        messageText.text = "";

    }

    private void GameStateStart()
    {
        startTimer -= Time.deltaTime;

        messageText.text = "Get Ready " + (int)(startTimer + 1);

        if (startTimer < 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            messageText.text = "";
            gameState = GameState.Playing;
            gamerTimer = gamerTimerAmount;
            startTimer = startTImerAmount;
            score = 0;

            highScorePanel.gameObject.SetActive(false);
            newGameButton.gameObject.SetActive(false);
            highScoresButton.gameObject.SetActive(false);

            player.SetActive(true);
            worldCamera.gameObject.SetActive(false);
        }
    }

    private void GameStatePlaying()
    {
        gamerTimer -= Time.deltaTime;
        int seconds = Mathf.RoundToInt(gamerTimer);
        timerText.text = string.Format("Time: {0:D2}:{1:D2}", (seconds / 60), (seconds % 60));

        if (gamerTimer <= 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
            //Debug.Log("Game Over Score: " + score);
            messageText.text = "Game Over! Score: " + score;
            messageText.text = "Press Enter to play again!";
            gameState = GameState.GameOver;
            player.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].gameObject.SetActive(false);
            }
            highScores.AddScore(score);
            highScores.SaveScoresToFile();
            newGameButton.gameObject.SetActive(true);
            highScoresButton.gameObject.SetActive(true);
        }

        //Timer before activating target.
        targetActivateTimer -= Time.deltaTime;
        if(targetActivateTimer <= 0)
        {
            ActivateRandomTarget();
            targetActivateTimer = targetActivateTimerAmount;
        }
    }
    private void GameStateGameOver()
    {
        if(Input.GetKeyUp(KeyCode.Return))
        {
            gameState = GameState.Start;
            timerText.text = "";
            scoreText.text = "";
        }
    }
}


