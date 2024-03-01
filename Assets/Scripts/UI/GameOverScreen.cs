using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
	private TextMeshProUGUI scoreText;

	private void Start()
	{
		InitializeScoreText();
	}

	private void InitializeScoreText()
	{
		if (transform.childCount > 1)
		{
			scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		}
	}

	public void GameOverScreenOn(int score)
	{
		//if game over screen is called before start method for some reason (actually happens a lot)
		if (scoreText == null)
		{
			InitializeScoreText();
		}

		gameObject.SetActive(true);
		scoreText.text = "Your score: " + score.ToString() + " points!";
	}

	public void RestartButton()
	{
		//// Get the name of the currently active scene and load it
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void MenuButton()
	{
		SceneManager.LoadScene("Main menu");
	}
}
