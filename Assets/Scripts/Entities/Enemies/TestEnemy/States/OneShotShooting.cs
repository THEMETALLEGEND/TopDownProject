using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotShooting : BaseState
{
	private TestEnemyStates _sm;
	private SpriteRenderer oneShotModel;
	private Coroutine _oneShotAttack;
	public OneShotShooting(TestEnemyStates enemyStateMachine) : base("OneShotShooting", enemyStateMachine)
	{
		_sm = (TestEnemyStates)stateMachine;
	}

	public override void Enter()
	{
		base.Enter();

		oneShotModel = _sm.transform.GetChild(0).GetComponent<SpriteRenderer>();

		if (_sm.playerObject != null)
		{
			;
			_sm.pointTarget.transform.position = _sm.enemyObject.transform.position;
			_sm.TargetSetter(_sm.pointTarget);

			_oneShotAttack = _sm.StartCoroutine(OneShot());
		}
	}

	IEnumerator OneShot()
	{
		while (true)
		{
			// ���������, ������������ �� ����� � ����� ������
			if (!_sm.CheckPlayerContact(48, 1, 30))
			{
				stateMachine.ChangeState(_sm.chasingState);
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

		// Check if the player is on the right or left of the object
		Vector3 directionToPlayer = _sm.playerObject.transform.position - _sm.transform.position;
		float angle = Vector3.SignedAngle(directionToPlayer, _sm.transform.right, Vector3.up);

		// Determine if the player is on the right or left
		bool isOnRightSide = angle >= 0 && angle <= 179;
		bool isOnLeftSide = angle >= 180 && angle <= 359;

		// Flip the object's model based on player position
		if (isOnRightSide)
		{
			oneShotModel.flipX = false; // Model is normal
		}
		else if (isOnLeftSide)
		{
			oneShotModel.flipX = true; // Model is inverted by X
		}

		if (!_sm.CheckPlayerContact(100, 5, 30)) //���� ������ ���������� ��������
			stateMachine.ChangeState(_sm.oneShotChasingState);
	}

	public override void Exit()
	{
		base.Exit();

		if (_oneShotAttack != null) //���� �������� ������ �������������
		{
			_sm.StopCoroutine(_oneShotAttack); //�� ������������� �
			_oneShotAttack = null;  //� ��������� ���������� null (�� ������� �������� �� ���������������)
		}
	}
}
