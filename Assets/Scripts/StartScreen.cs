using UnityEngine;

public class StartScreen : MonoBehaviour
{
	[SerializeField] private Animator _CreditsAnimator;
	private bool _isCreditsVisible = false;
	private AudioSource _dayBGM;
	private AudioSource _nightBGM;
	private void Start()
	{
        _dayBGM = GameObject.Find("DayBGM").GetComponent<AudioSource>();
        _nightBGM = GameObject.Find("NightBGM").GetComponent<AudioSource>();
        _dayBGM.volume = 0.3f;
        _nightBGM.volume = 0.0f;
    }
    public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Story");
    }
	public void QuitGame()
	{
		Application.Quit();
    }
	public void Enter()
	{
		if (_isCreditsVisible)
		{
			_CreditsAnimator.SetTrigger("Out");
			_isCreditsVisible = false;
		}
		else
		{
			_CreditsAnimator.SetTrigger("Enter");
			_isCreditsVisible = true;

		}
    }
}
