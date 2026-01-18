using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _lifeText;
    private AudioSource _dayBGM;
    private AudioSource _nightBGM;


    private void Start()
    {
        _dayBGM = GameObject.Find("DayBGM").GetComponent<AudioSource>();
        _nightBGM = GameObject.Find("NightBGM").GetComponent<AudioSource>();
        _dayBGM.volume = 0.3f;
        _nightBGM.volume = 0.0f;
        int times = PlayerPrefs.GetInt("SessionTime", 0);
        int lifes = PlayerPrefs.GetInt("LifeLeft", 0);
        int score = PlayerPrefs.GetInt("Score", 0);
        _scoreText.text = score.ToString();
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
