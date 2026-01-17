#region Info
// **********************************************************************
// StartScreen.cs by Fulm1na
// 		Elite-Staffel
// **********************************************************************
#endregion

using UnityEngine;

public class StartScreen : MonoBehaviour
{
	[SerializeField] private Animator _CreditsAnimator;
	private bool _isCreditsVisible = false;
    public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
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
