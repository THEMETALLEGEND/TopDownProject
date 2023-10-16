using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitting : BaseState
{

    private TestEnemyStates _sm;
    private Coroutine _strikeCoroutine; //здесь объявляем переменную корутины ShootBurst 
    public EnemyHitting(TestEnemyStates enemyStateMachine) : base("TestEnemyHitting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        //_sm.aIPath.canMove = false;
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + (new Vector3(direction.x, direction.y, 0f) * 2f); //ставим пустое ГО под ноги агенту но немного вперед в сторону игрока чтобы он
                                                                                                                                    //остановился и обнулил desiredVelocity и все еще был повернут к игроку лицом
        _sm.TargetSetter(_sm.pointTarget);

        _strikeCoroutine = _sm.StartCoroutine(StrikeEnum()); //назначаем корутину и вызываем ее
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(_sm.shootingPlayerDistanceExit)) //если дальше указанного значения
            stateMachine.ChangeState(_sm.chasingState);
    }

    IEnumerator StrikeEnum()
    {
        while (true)
        {
            // проверяем, сталкивается ли агент с лучом игрока
            if (!_sm.CheckPlayerContact(48, 1, 30))
            {
                stateMachine.ChangeState(_sm.chasingState);
                yield break;
            }

            // если агент сталкивается с лучом, то продолжаем стрельбу
            Strike();
            yield return new WaitForSeconds(_sm.strikeTiming);
        }
    }

    private void Strike()
    {
        _sm.aIPath.maxSpeed = _sm.dodgeSpeed; //на время доджа сильно увеличиваем скорость
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; //высчитываем нормализованный вектор от агента до игрока

        Vector3 dodgePoint = direction * 20; //ставим точку за игроком
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + dodgePoint; //ставим таргет ГО на точку за игроком
    }

    public override void Exit()
    {
        base.Exit();

        if (_strikeCoroutine != null) //если корутина сейчас проигрывается
        {
            _sm.StopCoroutine(_strikeCoroutine); //то останавливаем её
            _strikeCoroutine = null;  //и назначаем переменной null (по другому корутина не останавливалась)
        }
    }
}