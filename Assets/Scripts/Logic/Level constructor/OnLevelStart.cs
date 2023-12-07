using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnLevelStart : MonoBehaviour
{
    public UnityEvent onLevelStart;
    private FillStatusBar playerHealthSlider;
    void Start()
    {
        playerHealthSlider = GameObject.Find("Player Health Slider").GetComponent<FillStatusBar>();
        onLevelStart?.Invoke();
    }
}
