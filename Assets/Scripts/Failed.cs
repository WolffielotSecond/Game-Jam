using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Failed : MonoBehaviour
{
    [SerializeField] private TMP_Text _reasonText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _lifeText;
    private AudioSource _dayBGM;
    private AudioSource _nightBGM;

    private void Start()
    {
        _dayBGM = GameObject.Find("DayBGM").GetComponent<AudioSource>();
        _nightBGM = GameObject.Find("NightBGM").GetComponent<AudioSource>();
        _dayBGM.volume = 0.0f;
        _nightBGM.volume = 0.3f;
        int times = PlayerPrefs.GetInt("SessionTime", 0);
        int lifes = PlayerPrefs.GetInt("LifeLeft", 0);
        _timeText.text = "Time Survived: " + times + "s";
        _lifeText.text = "Life Left: " + lifes;
        if (lifes <= 0)
        {
            _reasonText.text = "Killed by Mobs";
        }
        else
        {
            _reasonText.text = "Timeout";
        }
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
