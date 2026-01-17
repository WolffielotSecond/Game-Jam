using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _lifeText;

    private void Start()
    {
        int times = PlayerPrefs.GetInt("SessionTime", 0);
        int lifes = PlayerPrefs.GetInt("LifeLeft", 0);
        int score = PlayerPrefs.GetInt("Score", 0);
        _scoreText.text = "Score: " + score;
        _timeText.text = "Time Survived: " + times + "s";
        _lifeText.text = "Life Left: " + lifes;
    }
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
    }
    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
}
