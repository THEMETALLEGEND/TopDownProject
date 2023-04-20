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
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position; //������ ������ �� ��� ���� ������ ����� �� ����������� � ������� desiredVelocity
        _sm.TargetSetter(_sm.pointTarget);

        _sm.StartCoroutine(ShootBurst()); //����� ��������� �������� ShootBurst 
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(20)) //���� ������ 35
            stateMachine.ChangeState(_sm.chasingState);

    }

    IEnumerator ShootBurst() //�������� ������� ������ ���-�� 3 ���� � ����� ���� 3 �������
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
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; // ��������� ������ ����������� �� ����� �� ������

        // ��������� �������, � ������� ����� ������� ����, ������� ������� �� �����.
        Vector2 offset = direction * 2.75f; 
        Vector3 position = new Vector3(_sm.enemyObject.transform.position.x + offset.x, _sm.enemyObject.transform.position.y + offset.y, 0f); //����� �������������� � vector3 � ����������� ������ z ����������

        GameObject bullet = Object.Instantiate(_sm.bulletPrefab, position, Quaternion.identity); // ������� ��������� ������� ���� � ������� ����� � � ������� ���������
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>(); // �������� ������ �� Rigidbody2D ���������� ����
        bulletRigidbody.AddForce(direction * 20f, ForceMode2D.Impulse); // ��������� ���� � ����������� ������, ��������� ����������� ������ ����������� � �������� ���� 20
    }

    public override void Exit()
    {
        base.Exit();

        _sm.StopCoroutine(ShootBurst()); //������������� ��������
    }
}
