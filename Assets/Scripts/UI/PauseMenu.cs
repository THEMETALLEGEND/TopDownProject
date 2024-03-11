using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private GameObject playerUI;
	private GameObject pauseMenu;
	private GameObject resumeButton;
	private GameObject menuButton;
	private GameObject playerObject;
	private CharacterController2D playerController;
	private GunController gunController;
	public static bool isPaused = false;

	private void Start()
	{
		playerUI = GameObject.Find("PlayerUI");
		pauseMenu = GameObject.Find("PauseMenu");
		playerObject = GameObject.Find("Player");
		playerController = playerObject.GetComponent<CharacterController2D>();
		gunController = playerObject.GetComponent<GunController>();
		pauseMenu.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("pressing esc");
			if (!isPaused)
			{
				PauseGame();
				Debug.Log("pausing");
			}
			else if (isPaused)
			{
				ResumeGame();
				Debug.Log("resuming");
			}
		}
	}

	public void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0f;
		playerController.enabled = false;
		gunController.enabled = false;
		playerUI.SetActive(false);
		pauseMenu.SetActive(true);
	}

	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1f;
		playerController.enabled = true;
		gunController.enabled = true;
		playerUI.SetActive(true);
		pauseMenu.SetActive(false);
	}

	public void ExitToMenu()
	{
		DeleteInventory();
		isPaused = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene("Main menu");
	}

	private void DeleteInventory()
	{
		GameObject inventory = GameObject.Find("Player Inventory");

		if (inventory != null)
			Destroy(inventory);
		else
			Debug.LogWarning("Player Inventory not found. Unable to delete.");
	}
}
