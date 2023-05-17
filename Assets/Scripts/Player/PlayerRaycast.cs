using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public LayerMask enemyLayer; // ����� ���� "Enemies"
    public LayerMask obstaclesLayer;
    public float rayLength = 10f; // ����� �����
    public int rayCount = 48; // ���������� �����
    public float angleStep = 7.5f; // ��� ����� ������ �����
    public bool showDebugGizmos; // ���������� �� ���������� �����

    private void Update()
    {
        // ������ 48 ����� ������ ������
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, enemyLayer | obstaclesLayer);

            // �������� ���� ��� ����� � ����������� �� ����, ���������� �� ��� � ��������
            Color lineColor = Color.green;
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                    lineColor = Color.red;
                else if (hit.collider.CompareTag("Obstacles"))
                    lineColor = Color.blue;
            }

            // ������ ����� ��� ����
            if (showDebugGizmos)
                Debug.DrawLine(transform.position, hit.collider != null ? hit.point : transform.position + direction * rayLength, lineColor);

            // ���� ��� ���������� � �������� �� ���� "Enemies", ������� ��������� � �������
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                Debug.Log("Ray collided with Enemy");
        }
    }
}
