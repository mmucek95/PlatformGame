using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public enum GameState
    {
        GS_PAUSEMENU,
        GS_GAME,
        GS_LEVELCOMPLETED,
        GS_GAME_OVER
    }

    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager insance;
    public Canvas inGameCanvas;
    public Text coinsText;
    private int coins = 0;
    public Image[] keysTab;
    public int keys = 0;
    public int lives = 3;
    public Image[] lifeTab;

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        inGameCanvas.enabled = (newGameState == GameState.GS_GAME);
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
        SetGameState(GameState.GS_GAME);
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
        insance = this;
        
        for(int i =0; i <keysTab.Length; i++)
        {
            keysTab[i].color = Color.gray;
        }
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void lostLife()
    {
        lifeTab[--lives].enabled = false;
        if(lives <=0)
        {
            SetGameState(GameState.GS_GAME_OVER);
        }
    }
}
