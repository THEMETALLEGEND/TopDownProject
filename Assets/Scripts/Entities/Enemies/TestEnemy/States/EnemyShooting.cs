using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : BaseState
{

    private TestEnemyStates _sm;
    private Coroutine _shootBurstCoroutine; //здесь объ€вл€ем переменную корутины ShootBurst 
    public EnemyShooting(TestEnemyStates enemyStateMachine) : base("TestEnemyShooting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        //_sm.aIPath.canMove = false;
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + (new Vector3(direction.x, direction.y, 0f) * 2f); //ставим пустое √ќ под ноги агенту но немного вперед в сторону игрока чтобы он
                                                                                                                                    //остановилс€ и обнулил desiredVelocity и все еще был повернут к игроку лицом
        _sm.TargetSetter(_sm.pointTarget);

        _shootBurstCoroutine = _sm.StartCoroutine(ShootBurst()); //назначаем корутину и вызываем ее
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(_sm.shootingPlayerDistanceExit)) //если дальше указанного значени€
            stateMachine.ChangeState(_sm.chasingState);

    }

    IEnumerator ShootBurst()
    {
        while (true)
        {
            // провер€ем, сталкиваетс€ ли агент с лучом игрока
            if (!_sm.CheckPlayerContact(48, 1, 30))
            {
                stateMachine.ChangeState(_sm.chasingState);
                yield break;
            }

            // если агент сталкиваетс€ с лучом, то продолжаем стрельбу
            Shoot();
            yield return new WaitForSeconds(_sm.shootingBurstShortTiming);
            Shoot();
            yield return new WaitForSeconds(_sm.shootingBurstShortTiming);
            Shoot();
            yield return new WaitForSeconds(_sm.shootingBurstShortTiming);
            Dodge();
            yield return new WaitForSeconds(_sm.shootingBurstLongTiming);
        }
    }


    private void Shoot()
    {
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; // ¬ычисл€ем вектор направлени€ от врага до игрока

        float accuracy = 0.2f; // ”станавливаем точность стрельбы
        float rand = Random.Range(-accuracy, accuracy); // √енерируем случайное смещение дл€ направлени€ выстрела

        Vector2 shootOffset = new Vector2(direction.y, -direction.x) * rand; //вычисл€ем перпендикул€рный вектор и выбираем по нему рандомную точку согласно rand
        Vector2 newDirection = direction + shootOffset; //новое направление пули с небольшой погрешностью

        // ¬ычисл€ем позицию, в которой нужно создать пулю, немного впереди от врага.
        Vector2 firePointOffset = newDirection * 3.5f; 
        Vector3 position = new Vector3(_sm.enemyObject.transform.position.x + firePointOffset.x, _sm.enemyObject.transform.position.y + firePointOffset.y, 0f); //€вное преобразование в vector3 с добавлением пустой z координаты

        GameObject bullet = Object.Instantiate(_sm.bulletPrefab, position, Quaternion.identity); // —оздаем экземпл€р префаба пули в позиции врага и с нулевым поворотом
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>(); // ѕолучаем ссылку на Rigidbody2D экземпл€ра пули
        bulletRigidbody.AddForce(direction * _sm.shootingBulletSpeed, ForceMode2D.Impulse); // ѕримен€ем силу в направлении игрока, использу€ вычисленный вектор направлени€ и мощность силы 20
    }

    private void Dodge()
    {
        _sm.aIPath.maxSpeed = _sm.dodgeSpeed; //на врем€ доджа сильно увеличиваем скорость
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; //высчитываем нормализованный вектор от агента до игрока
        Vector2 perpendicularVector = new Vector2(direction.y, -direction.x); //высчитываем вектор перпендикул€рный вектору выше

        int dodgeDirectionDecision = Random.Range(0, 2); //рандомно решаем бежать влево или вправо
        if (dodgeDirectionDecision == 0) //если влево
        {
            _sm.dodgeRandom = Random.Range(-10f, -5f);
        }
        else //если вправо
        {
            _sm.dodgeRandom = Random.Range(5f, 10f);
        }

        Vector3 dodgePoint = perpendicularVector * _sm.dodgeRandom; //ставим точку на перпендикул€рный вектор
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + dodgePoint; //ставим таргет √ќ на ранд точку перпендикул€рного вектора относительно себ€
    }

    public override void Exit()
    {
        base.Exit();

        if (_shootBurstCoroutine != null) //если корутина сейчас проигрываетс€
        {
            _sm.StopCoroutine(_shootBurstCoroutine); //то останавливаем еЄ
            _shootBurstCoroutine = null;  //и назначаем переменной null (по другому корутина не останавливалась)
        }
    }
}
