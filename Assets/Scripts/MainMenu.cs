using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text scoreText;
    public GameObject howToPlayPanel;
    void Start()
    {
        DontDestroy.instance.gameObject.GetComponent<AudioSource>().Play();
        float highscore = PlayerPrefs.GetFloat("highscore");
        scoreText.text = Mathf.Floor(highscore).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run(){
        SceneManager.LoadScene("MainScene");
    }

    public void Exit(){
        Application.Quit();
    }

    public void HowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void Back()
    {
        howToPlayPanel.SetActive(false);
    }
}
