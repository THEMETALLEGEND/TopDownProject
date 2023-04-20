using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : BaseState
{

    private TestEnemyStates _sm;
    public EnemyShooting(TestEnemyStates enemyStateMachine) : base("TestEnemyShooting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        //_sm.aIPath.canMove = false;
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position; //ставим пустое ГО под ноги агенту чтобы он остановился и обнулил desiredVelocity
        _sm.TargetSetter(_sm.pointTarget);

        _sm.StartCoroutine(ShootBurst()); //здесь объявляем корутину ShootBurst 
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(20)) //если дальше 35
            stateMachine.ChangeState(_sm.chasingState);

    }

    IEnumerator ShootBurst() //корутина которая делает что-то 3 раза и потом ждет 3 секунды
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f);
            Shoot();
            yield return new WaitForSeconds(0.2f);
            Shoot();
            yield return new WaitForSeconds(0.2f);
            yield return new WaitForSeconds(1.8f);
        }
    }

    private void Shoot()
    {
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; // Вычисляем вектор направления от врага до игрока

        // Вычисляем позицию, в которой нужно создать пулю, немного впереди от врага.
        Vector2 offset = direction * 2.75f; 
        Vector3 position = new Vector3(_sm.enemyObject.transform.position.x + offset.x, _sm.enemyObject.transform.position.y + offset.y, 0f); //явное преобразование в vector3 с добавлением пустой z координаты

        GameObject bullet = Object.Instantiate(_sm.bulletPrefab, position, Quaternion.identity); // Создаем экземпляр префаба пули в позиции врага и с нулевым поворотом
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>(); // Получаем ссылку на Rigidbody2D экземпляра пули
        bulletRigidbody.AddForce(direction * 20f, ForceMode2D.Impulse); // Применяем силу в направлении игрока, используя вычисленный вектор направления и мощность силы 20
    }

    public override void Exit()
    {
        base.Exit();

        _sm.StopCoroutine(ShootBurst()); //останавливаем корутину
    }
}
