using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotChasing : BaseState
{
	private TestEnemyStates _sm;
	private Coroutine _oneShot;
	public OneShotChasing(TestEnemyStates enemyStateMachine) : base("OneShotChasing", enemyStateMachine)
	{
		_sm = (TestEnemyStates)stateMachine;
	}

	public override void Enter()
	{
		base.Enter();

		if (_sm.playerObject != null)
		{
			_sm.TargetSetter(_sm.playerObject);
			_sm.aIPath.maxSpeed = _sm.slowSpeed;

			_oneShot = _sm.StartCoroutine(OneShot());
		}
	}

	IEnumerator OneShot()
	{
		while (true)
		{
			// проверяем, сталкивается ли агент с лучом игрока
			if (!_sm.CheckPlayerContact(48, 1, 30))
			{
				//stateMachine.ChangeState(_sm.chasingState);
				yield break;
			}

			// если агент сталкивается с лучом, то продолжаем стрельбу
			yield return new WaitForSeconds(_sm.oneShotTiming);
			Shoot();
		}
	}

	private void Shoot()
	{
		Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; // Вычисляем вектор направления от врага до игрока

		float accuracy = 0.2f; // Устанавливаем точность стрельбы
		float rand = Random.Range(-accuracy, accuracy); // Генерируем случайное смещение для направления выстрела

		Vector2 shootOffset = new Vector2(direction.y, -direction.x) * rand; //вычисляем перпендикулярный вектор и выбираем по нему рандомную точку согласно rand
		Vector2 newDirection = direction + shootOffset; //новое направление пули с небольшой погрешностью

		// Вычисляем позицию, в которой нужно создать пулю, немного впереди от врага.
		Vector2 firePointOffset = newDirection * 3.5f;
		Vector3 position = new Vector3(_sm.enemyObject.transform.position.x + firePointOffset.x, _sm.enemyObject.transform.position.y + firePointOffset.y, 0f); //явное преобразование в vector3 с добавлением пустой z координаты

		GameObject bullet = Object.Instantiate(_sm.bulletPrefab, position, Quaternion.identity); // Создаем экземпляр префаба пули в позиции врага и с нулевым поворотом
		Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>(); // Получаем ссылку на Rigidbody2D экземпляра пули
		bulletRigidbody.AddForce(direction * _sm.shootingBulletSpeed, ForceMode2D.Impulse); // Применяем силу в направлении игрока, используя вычисленный вектор направления и мощность силы 20
	}

	public override void UpdateLogic()
	{
		base.UpdateLogic();

		_sm.SetAlerted(true);



		if (!_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceExit)) //если дальше указанного значения
			stateMachine.ChangeState(_sm.roamingState);

		if (_sm.CheckPlayerContact(100, 5, 30)) //если ближе указанного значения
			stateMachine.ChangeState(_sm.oneShotShootingState);
	}

	public override void Exit()
	{
		base.Exit();

		if (_oneShot != null) //если корутина сейчас проигрывается
		{
			_sm.StopCoroutine(_oneShot); //то останавливаем её
			_oneShot = null;  //и назначаем переменной null (по другому корутина не останавливалась)
		}
	}
}
