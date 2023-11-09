using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public GameObject[] enemies;
    private TriggerCollider triggerCollider;

    private enum State
    {
        Idle,
        Active,
    }

    private State state;

    private void Awake()
    {
        state = State.Idle;
    }
    private void Start()
    {
        triggerCollider = GetComponent<TriggerCollider>();
        triggerCollider.OnPlayerTriggerEnter += TriggerCollider_OnPlayerTriggerEnter;
    }

    private void TriggerCollider_OnPlayerTriggerEnter(object sender, System.EventArgs e)
    {
        if (state == State.Idle)
        {
            StartBattle();
            triggerCollider.OnPlayerTriggerEnter -= TriggerCollider_OnPlayerTriggerEnter; //optimisation
        }
    }

    private void StartBattle()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(enabled);
        }
        state = State.Active;
    }
}
