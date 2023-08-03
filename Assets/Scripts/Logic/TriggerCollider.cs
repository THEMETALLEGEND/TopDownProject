using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    public event EventHandler OnPlayerTriggerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerTriggerEnter?.Invoke(this, EventArgs.Empty);
        }
    }
}
