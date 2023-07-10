using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string keyId; // ID �����, ������������ ��� �������� �����
    private BoxCollider2D col;

    public GameObject modelOpen;
    public GameObject modelClosed;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();

        DoorModelCheckOnStart();
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

        DoorModelSwitch();
    }

    public void DoorModelCheckOnStart()
    {
        // ���������, ���� �� �������� ������ Model_Open
        Transform childOpen = transform.Find("Model_Open");
        if (childOpen != null)
        {
            modelOpen = childOpen.gameObject;
        }

        // ���������, ���� �� �������� ������ Model_Closed
        Transform childClosed = transform.Find("Model_Closed");
        if (childClosed != null)
        {
            modelClosed = childClosed.gameObject;
        }
    }

    public void DoorModelSwitch()
    {
        // ���������, ���������� �� �������� �������
        if (modelOpen != null && modelClosed != null)
        {
            // ���������, �������� �� ���� Model_Open
            if (modelOpen.activeSelf)
            {
                // ��������� ���� Model_Open
                modelOpen.SetActive(false);
                // �������� ���� Model_Closed
                modelClosed.SetActive(true);
            }
            else
            {
                // ��������� ���� Model_Closed
                modelClosed.SetActive(false);
                // �������� ���� Model_Open
                modelOpen.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("����������� �������� ������� Model_Open � Model_Closed.");
        }
    }
}