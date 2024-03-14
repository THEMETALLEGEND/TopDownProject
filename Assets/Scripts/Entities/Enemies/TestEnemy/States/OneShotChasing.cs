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
			// ���������, ������������ �� ����� � ����� ������
			if (!_sm.CheckPlayerContact(48, 1, 30))
			{
				//stateMachine.ChangeState(_sm.chasingState);
				yield break;
			}

			// ���� ����� ������������ � �����, �� ���������� ��������
			yield return new WaitForSeconds(_sm.oneShotTiming);
			Shoot();
		}
	}

	private void Shoot()
	{
		Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; // ��������� ������ ����������� �� ����� �� ������

		float accuracy = 0.2f; // ������������� �������� ��������
		float rand = Random.Range(-accuracy, accuracy); // ���������� ��������� �������� ��� ����������� ��������

		Vector2 shootOffset = new Vector2(direction.y, -direction.x) * rand; //��������� ���������������� ������ � �������� �� ���� ��������� ����� �������� rand
		Vector2 newDirection = direction + shootOffset; //����� ����������� ���� � ��������� ������������

		// ��������� �������, � ������� ����� ������� ����, ������� ������� �� �����.
		Vector2 firePointOffset = newDirection * 3.5f;
		Vector3 position = new Vector3(_sm.enemyObject.transform.position.x + firePointOffset.x, _sm.enemyObject.transform.position.y + firePointOffset.y, 0f); //����� �������������� � vector3 � ����������� ������ z ����������

		GameObject bullet = Object.Instantiate(_sm.bulletPrefab, position, Quaternion.identity); // ������� ��������� ������� ���� � ������� ����� � � ������� ���������
		Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>(); // �������� ������ �� Rigidbody2D ���������� ����
		bulletRigidbody.AddForce(direction * _sm.shootingBulletSpeed, ForceMode2D.Impulse); // ��������� ���� � ����������� ������, ��������� ����������� ������ ����������� � �������� ���� 20
	}

	public override void UpdateLogic()
	{
		base.UpdateLogic();

		_sm.SetAlerted(true);



		if (!_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceExit)) //���� ������ ���������� ��������
			stateMachine.ChangeState(_sm.roamingState);

		if (_sm.CheckPlayerContact(100, 5, 30)) //���� ����� ���������� ��������
			stateMachine.ChangeState(_sm.oneShotShootingState);
	}

	public override void Exit()
	{
		base.Exit();

		if (_oneShot != null) //���� �������� ������ �������������
		{
			_sm.StopCoroutine(_oneShot); //�� ������������� �
			_oneShot = null;  //� ��������� ���������� null (�� ������� �������� �� ���������������)
		}
	}
}
