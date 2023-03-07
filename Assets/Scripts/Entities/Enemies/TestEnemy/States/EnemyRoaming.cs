using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoaming : BaseState
{

    private TestEnemyStates _sm;
    public EnemyRoaming(TestEnemyStates enemyStateMachine) : base("TestEnemyRoaming", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
    }
    //отправляем enemyroaming со стейт машиной которую называем enemystatemachine на родителя (блюпринт состояния) с названием состояния и уже названной стейт машиной.
    //потом присваиваем нужную стейтмашину в переменную чтобы не вводить ее каждый раз
    
    private bool timerEnded = false;

    public override void Enter()
    {
        base.Enter();

        _sm.startingPosition = _sm.transform.position; //стартовая позиция для отсчета состояния roaming
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown("f"))
            stateMachine.ChangeState(_sm.waitingState); //меняем состояние по кнопке
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.aIPath.reachedDestination == true) //если таймер не закончен И пришли к цели
        {
            _sm.roamingInterval -= 1; //начинаем отсчет таймера
        }

        if (_sm.roamingInterval <= 0f) //если таймер равен или меньше нулю, то он закончен
        {
            timerEnded = true;
        }



        if (timerEnded == true && _sm.aIPath.reachedDestination == true) //если таймер закончен И пришли к цели
        {
            _sm.aIDest.target.position = GetRoamingPosition(); //берем рандомную цель в небольшом радиусе
            _sm.roamingInterval = 60f; //радиус
            timerEnded = false; //таймер больше не закончен, запускаем заново
        }
    }


    private Vector3 GetRoamingPosition()
    {
        return _sm.startingPosition + Random.insideUnitSphere * _sm.roamRadius; //от стартовой позиции делаем сферу радиуса уже назначенного
    }

    /*
    private TestEnemy testEnemy;
    private AIPath aIPath;
    private AIDestinationSetter aIDest;
    private Vector3 target;
    private Vector3 startingPosition;

    public float roamRadius = 10f;

    void Start()
    {
        startingPosition = transform.position;
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Random.insideUnitSphere * roamRadius;
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            aIDest.target.position = GetRoamingPosition();
        }
    }
    */
}
