using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Failed : MonoBehaviour
{
    [SerializeField] private TMP_Text _reasonText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _lifeText;

    private void Start()
    {
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
