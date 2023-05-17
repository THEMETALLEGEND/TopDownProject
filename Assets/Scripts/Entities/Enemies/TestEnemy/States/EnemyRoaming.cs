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
        _sm.TargetSetter(_sm.pointTarget);    //назначение цели 
        _sm.aIDest.target.position = GetRoamingPosition();      //начальная рандомизация позиции цели от стартовой точки
        _sm.aIPath.maxSpeed = _sm.roamingSpeed;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // РИСОВКА ЛУЧЕЙ И ПЕРЕКЛЮЧЕНИЕ СОСТОЯНИЙ
        for (int i = 0; i < _sm.rayCount; i++)
        {
            // вычисляем угол луча
            float angle = i * _sm.angleStep;

            // вычисляем направление луча на основе угла
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            // выпускаем луч и получаем информацию о столкновении с объектами на слоях obstacleLayer и playerLayer
            RaycastHit2D hit = Physics2D.Raycast(_sm.transform.position, direction, _sm.rayLength, _sm.obstacleLayer | _sm.playerLayer);

            // выбираем цвет для линии на основе столкновения луча с объектом
            Color lineColor = hit.collider != null ? hit.collider.CompareTag("Player") ? Color.red : Color.yellow : Color.green;

            // рисуем линию для луча
            if (_sm.showDebugGizmos)
                Debug.DrawLine(_sm.transform.position, hit.collider != null ? hit.point : _sm.transform.position + direction * _sm.rayLength, lineColor);

            // если луч столкнулся с игроком, переходим в другое состояние
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // переключаемся в другое состояние в зависимости от того, является ли агент NPC
                if (_sm.isAnNPC && !_sm.debugMode)
                    stateMachine.ChangeState(_sm.fleeingState);
                else if (!_sm.debugMode)
                    stateMachine.ChangeState(_sm.chasingState);
            }
        }

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
