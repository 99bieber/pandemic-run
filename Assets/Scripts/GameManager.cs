using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject pausePanel;
    public Player player;
    public Text scoreTextPaused;
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();

    }

    void Update()
    {
        scoreTextPaused.text = Mathf.Floor(scoreManager.score).ToString();
    }

    public void Pause()
    {
        
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        pausePanel.SetActive(false);
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }


}
