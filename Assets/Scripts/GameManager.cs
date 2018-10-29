using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public enum GameState
    {
        GS_PAUSEMENU,
        GS_GAME,
        GS_LEVELCOMPLETED,
        GS_GAME_OVER
    }

    public GameState currentGameState = GameState.GS_GAME;
    public static GameManager insance;
    public Canvas inGameCanvas;
    public Text coinsText;
    private int coins = 0;
    public Image[] keysTab;
    public int keys = 0;
    public int lives = 3;
    public Image[] lifeTab;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompleted;
    public Canvas gameOverCanvas;
    public Text finalScoreText;

    public Text finalGems;
    public Text finalLives;

    public Text highScoreText;
    public Text scoreText;

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;

        if (newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Poziom1")
            {
                int score = lives * 20 + coins * 10;
                if (PlayerPrefs.GetInt("HighscoreLevel1") < score)
                    PlayerPrefs.SetInt("HighscoreLevel1", score);
                highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel1");
                scoreText.text = "score: " + score;
            }
            else if(currentScene.name == "Poziom2")
            {
                int score = lives * 20 + coins * 10;
                if (PlayerPrefs.GetInt("HighscoreLevel2") < score)
                    PlayerPrefs.SetInt("HighscoreLevel2", score);
                highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel2");
                scoreText.text = "score: " + score;
            }
        }

        inGameCanvas.enabled = (newGameState == GameState.GS_GAME);
        pauseMenuCanvas.enabled = (newGameState == GameState.GS_PAUSEMENU);
        levelCompleted.enabled = (newGameState == GameState.GS_LEVELCOMPLETED);
        gameOverCanvas.enabled = (newGameState == GameState.GS_GAME_OVER);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
        finalGems.text = (coins).ToString() + " x10";
        finalLives.text = (lives).ToString() + " x20";
    }

    public void addCoins()
    {
        coins++;
        coinsText.text = coins.ToString();
    }

    public void addKeys()
    {
        keysTab[keys++].color = Color.white;
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("HighscoreLevel1"))
            PlayerPrefs.SetInt("HighscoreLevel1", 0);

        if (!PlayerPrefs.HasKey("HighscoreLevel2"))
            PlayerPrefs.SetInt("HighscoreLevel2", 0);
 
        insance = this;
        InGame();
        for(int i = 0; i <keysTab.Length; i++)
        {
            keysTab[i].color = Color.gray;
        }
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (currentGameState == GameState.GS_PAUSEMENU))
            InGame();
        else if (Input.GetKeyDown(KeyCode.Escape) && (currentGameState == GameState.GS_GAME))
            PauseMenu();
    }

    public void lostLife()
    {
        lifeTab[--lives].enabled = false;
        if(lives <=0)
        {
            insance.GameOver();
        }
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene("Poziom2");
    }
}
