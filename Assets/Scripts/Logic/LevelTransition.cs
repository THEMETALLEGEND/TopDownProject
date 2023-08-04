using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public int transitionSceneIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hitbox")
        {
            SceneManager.LoadScene(transitionSceneIndex, LoadSceneMode.Single);
        }
    }
}
