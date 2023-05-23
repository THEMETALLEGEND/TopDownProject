using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string keyId; // ID �����, ������������ ��� �������� �����
    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        col.enabled = false;
        Debug.Log("opened");

        // �������� ��������� AstarPath � ������� A* � �����
        AstarPath astarPath = GameObject.FindObjectOfType<AstarPath>();

        // ���� ����� ��������� AstarPath, �������� ����� Scan()
        if (astarPath != null)
        {
            astarPath.Scan();
        }
    }
}