using System.Collections.Generic;
using UnityEngine;

public class StoryBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> _storyboards;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _previousButton;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _skipButton;

    private int _currentIndex = 0;
    public void Next()
	{
        _currentIndex++;
        _storyboards[_currentIndex].SetActive(true);
        if (_currentIndex == _storyboards.Count - 1)
        {
            _nextButton.SetActive(false);
            _startButton.SetActive(true);
            _skipButton.SetActive(false);
        }
        if (_currentIndex > 0)
        {
            _previousButton.SetActive(true);
        }
    }
    public void Previous() 
    {
        _storyboards[_currentIndex].SetActive(false);
        _currentIndex--;
        if (_currentIndex == 0)
        {
            _previousButton.SetActive(false);
        }
        if (_currentIndex < _storyboards.Count - 1)
        {
            _nextButton.SetActive(true);
            _startButton.SetActive(false);
            _skipButton.SetActive(true);
        }
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
    }
}
