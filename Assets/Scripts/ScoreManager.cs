using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score = 0;
    public Text scoreText;
    public EnergyBar energyBar;

    void Start()
    {

    }

    void Update()
    {
        score += 10f * Time.deltaTime;
        scoreText.text = Mathf.Floor(score).ToString();
        float highscore = PlayerPrefs.GetFloat("highscore");
        if (energyBar.energy == 0)
        {
            this.enabled = false;
            PlayerPrefs.SetFloat("currentscore", score);
        }
        if (score > highscore)
        {
            PlayerPrefs.SetFloat("highscore", score);
        }
        else
        {
            return;
        }
    }

}
