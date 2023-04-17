using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoaming : BaseState
{

    private TestEnemyStates _sm;
    public EnemyRoaming(TestEnemyStates enemyStateMachine) : base("TestEnemyRoaming", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
    }
    //отправл€ем enemyroaming со стейт машиной которую называем enemystatemachine на родител€ (блюпринт состо€ни€) с названием состо€ни€ и уже названной стейт машиной.
    //потом присваиваем нужную стейтмашину в переменную чтобы не вводить ее каждый раз
    
    private bool timerEnded = false;

    public override void Enter()
    {
        base.Enter();

        _sm.startingPosition = _sm.transform.position; //стартова€ позици€ дл€ отсчета состо€ни€ roaming
        _sm.TargetSetter(_sm.roamingTarget);    //назначение цели 
        _sm.aIDest.target.position = GetRoamingPosition();      //начальна€ рандомизаци€ позиции цели от стартовой точки
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown("f"))
            stateMachine.ChangeState(_sm.waitingState); //мен€ем состо€ние по кнопке

        _sm.CheckPlayerInRange(45); //провер€ем дистанцию до игрока (метод в стейтмашине)
        if (_sm.CheckPlayerInRange(45))   
        {
            stateMachine.ChangeState(_sm.chasingState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.aIPath.reachedDestination == true) //если таймер не закончен » пришли к цели
        {
            _sm.roamingInterval -= 1; //начинаем отсчет таймера
        }

        if (_sm.roamingInterval <= 0f) //если таймер равен или меньше нулю, то он закончен
        {
            timerEnded = true;
        }



        if (timerEnded == true && _sm.aIPath.reachedDestination == true) //если таймер закончен » пришли к цели
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
}
