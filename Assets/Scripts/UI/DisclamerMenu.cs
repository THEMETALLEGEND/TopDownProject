using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisclamerMenu : MonoBehaviour
{
	public void ExitToMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("M1 Main menu");
	}
}
