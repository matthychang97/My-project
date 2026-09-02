using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Reference to overlay Text to display winning text, etc
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

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

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    private GameState gameState;
    public GameState State { get { return gameState; } }

    private void Awake()
    {
        gameState = GameState.GameOver;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
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
    }

    private void GameStateStart()
    {
        startTimer -= Time.deltaTime;

        messageText.text = "Get Ready " + (int)(startTimer + 1);

        if (startTimer < 0)
        {
            messageText.text = "";
            gameState = GameState.Playing;
            gamerTimer = gamerTimerAmount;
            startTimer = startTImerAmount;
            score = 0;

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
            //Debug.Log("Game Over Score: " + score);
            messageText.text = "Game Over! Score: " + score;
            gameState = GameState.GameOver;
            player.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].gameObject.SetActive(false);
            }
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


