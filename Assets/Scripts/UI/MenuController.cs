using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	void Start()
	{
		// Get references to the buttons
		GameObject startButton = transform.Find("Start Button").gameObject;
		GameObject exitButton = transform.Find("Exit Button").gameObject;

		// Add click event listeners
		startButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartButtonOnClick);
		exitButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ExitButtonOnClick);
	}

	// Method to handle StartButton click
	void StartButtonOnClick()
	{
		// Load the Level 1 scene
		SceneManager.LoadScene("L1 Level 1");
	}

	// Method to handle ExitButton click
	void ExitButtonOnClick()
	{
		// Exit the game
		Application.Quit();
	}
}