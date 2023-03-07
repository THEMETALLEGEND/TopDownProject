using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    

    private void Start()
    {
        currentState = GetInitialState(); //����� ��������� ��������� ����������� �������
        if (currentState != null) //� ���� ��������� ��������� ������������� ����
            currentState.Enter(); //�� ������ � ��� ��������� �����
    }

    private void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic(); //������ �� ����� �������������� ������ ����
    }

    private void FixedUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics(); //������ �� ����� �������������� ������ ������������� ����
    }

    public void ChangeState(BaseState newState) //����� ����� ������ ��������� ��������� ���� ����� ���������
    {
        currentState.Exit(); //����������� ����� ������ � ������ ���������

        currentState = newState; //��������� � ����� ���������
        currentState.Enter(); //����������� ��������� ����� ���
    }

    protected virtual BaseState GetInitialState() //����� ���������� ��������� - ����� ������, ��� ������ ����� ������ ������ (����� �����������)
    {
        return null;
    }

    private void OnGUI() //��� ������ ��������� � ������� ����� ����
    {
        string content = currentState != null ? currentState.name : "(no current state)"; //���� currentstate �� null �� ����������� ��� ��������, � ���� null �� ����������� no current state
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>"); //������ � ���� ������� �������

    }
}
