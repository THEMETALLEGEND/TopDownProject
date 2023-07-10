using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : BaseState
{

    private TestEnemyStates _sm;
    private Coroutine _shootBurstCoroutine; //����� ��������� ���������� �������� ShootBurst 
    public EnemyShooting(TestEnemyStates enemyStateMachine) : base("TestEnemyShooting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        //_sm.aIPath.canMove = false;
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + (new Vector3(direction.x, direction.y, 0f) * 2f); //������ ������ �� ��� ���� ������ �� ������� ������ � ������� ������ ����� ��
                                                                                                                                    //����������� � ������� desiredVelocity � ��� ��� ��� �������� � ������ �����
        _sm.TargetSetter(_sm.pointTarget);

        _shootBurstCoroutine = _sm.StartCoroutine(ShootBurst()); //��������� �������� � �������� ��
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(_sm.shootingPlayerDistanceExit)) //���� ������ ���������� ��������
            stateMachine.ChangeState(_sm.chasingState);

    }

    IEnumerator ShootBurst()
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

    private void Dodge()
    {
        _sm.aIPath.maxSpeed = _sm.dodgeSpeed; //�� ����� ����� ������ ����������� ��������
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; //����������� ��������������� ������ �� ������ �� ������
        Vector2 perpendicularVector = new Vector2(direction.y, -direction.x); //����������� ������ ���������������� ������� ����

        int dodgeDirectionDecision = Random.Range(0, 2); //�������� ������ ������ ����� ��� ������
        if (dodgeDirectionDecision == 0) //���� �����
        {
            _sm.dodgeRandom = Random.Range(-10f, -5f);
        }
        else //���� ������
        {
            _sm.dodgeRandom = Random.Range(5f, 10f);
        }

        Vector3 dodgePoint = perpendicularVector * _sm.dodgeRandom; //������ ����� �� ���������������� ������
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + dodgePoint; //������ ������ �� �� ���� ����� ����������������� ������� ������������ ����
    }

    public override void Exit()
    {
        base.Exit();

        if (_shootBurstCoroutine != null) //���� �������� ������ �������������
        {
            _sm.StopCoroutine(_shootBurstCoroutine); //�� ������������� �
            _shootBurstCoroutine = null;  //� ��������� ���������� null (�� ������� �������� �� ���������������)
        }
    }
}
