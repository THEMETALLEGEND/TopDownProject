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

        //_sm.isAlerted = false;
        _sm.startingPosition = _sm.transform.position; //стартовая позиция для отсчета состояния roaming
        _sm.TargetSetter(_sm.pointTarget);    //назначение цели 
        _sm.aIDest.target.position = GetRoamingPosition();      //начальная рандомизация позиции цели от стартовой точки
        _sm.aIPath.maxSpeed = _sm.roamingSpeed;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // --------- РИСОВКА ЛУЧЕЙ И ПЕРЕКЛЮЧЕНИЕ СОСТОЯНИЙ

        //Поле зрения и детект игрока
        if (_sm.CheckPlayerContact(48, 1, 30))
        {
            // переключаемся в другое состояние в зависимости от того, является ли агент NPC
            if (!_sm.isAnNPC && !_sm.debugMode)
                stateMachine.ChangeState(_sm.chasingState);
        }

        //если в состоянии страха то в зависимости от поведения сменить состояние
        if (_sm.isAlerted && !_sm.isAnNPC)
            _sm.ChangeState(_sm.chasingState);
        else if (_sm.isAlerted && _sm.isAnNPC)// && _sm.CheckPlayerContact(48, 1, 30))
            _sm.ChangeState(_sm.fleeingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.aIPath.reachedEndOfPath == true) //если таймер не закончен И пришли к цели
        {
            _sm.roamingInterval -= 1; //начинаем отсчет таймера
        }

        if (_sm.roamingInterval <= 0f) //если таймер равен или меньше нулю, то он закончен
        {
            timerEnded = true;
        }



        if (timerEnded == true && _sm.aIPath.reachedEndOfPath == true) //если таймер закончен И пришли к цели
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
