using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    private GameObject agent;

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

    public void GetGameObject(GameObject agent)
    {
        this.agent = agent;
    }

    private void OnGUI()
    {
        if (agent != null)
        {
            Vector3 agentPosition = agent.transform.position; // �������� ������� ������
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(agentPosition); // ��������� ������� ������ � ������� �� ������
            float yOffset = 50f; // �������� �� ��� Y ������������ ������
            Vector2 labelPosition = new Vector2(screenPosition.x, Screen.height - screenPosition.y - yOffset); // ������� ������ �� ������

            string content = currentState != null ? currentState.name : "(no current state)";

            GUI.Label(new Rect(labelPosition.x - 50, labelPosition.y, 200, 50), content); // ���������� ����� ��� �������
        }
        
    }

    /*private void OnGUI() //��� ������ ��������� � ������� ����� ����
    {
        string content = currentState != null ? currentState.name : "(no current state)"; //���� currentstate �� null �� ����������� ��� ��������, � ���� null �� ����������� no current state
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>"); //������ � ���� ������� �������

    }*/
}
