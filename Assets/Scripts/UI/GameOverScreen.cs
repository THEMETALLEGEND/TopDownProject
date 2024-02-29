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
		scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		Debug.Log(scoreText);
	}

	public void GameOverScreenOn(int score)
	{
		gameObject.SetActive(true);
		scoreText.text = "Your score: " + score.ToString() + " points!";
	}

	public void RestartButton()
	{
		SceneManager.LoadScene("Zombie test level");
	}
	public void MenuButton()
	{
		SceneManager.LoadScene("Level 1");
	}

}
