using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }

    public void onLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("Poziom1"));
    }

    public void onLevel2ButtonPressed()
    {
        StartCoroutine(StartGame("Poziom2"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
