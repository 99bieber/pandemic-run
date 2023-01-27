using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text highscoreText;
    public Text currentScoreText;
    void Start()
    {
        float highscore = PlayerPrefs.GetFloat("highscore");
        float currentScore = PlayerPrefs.GetFloat("currentscore");
        highscoreText.text = Mathf.Floor(highscore).ToString();
        currentScoreText.text = Mathf.Floor(currentScore).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu() 
    {
        Debug.Log("mainmenu");
        SceneManager.LoadScene("MainMenu");
    }
}
