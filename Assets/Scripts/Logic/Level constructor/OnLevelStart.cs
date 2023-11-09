using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnLevelStart : MonoBehaviour
{
    public UnityEvent onLevelStart;
    void Start()
    {
        onLevelStart?.Invoke();
    }
}
