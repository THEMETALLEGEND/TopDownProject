using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDuplicateCheck : MonoBehaviour
{
    private void Start()
    {
        CheckIfCanvasExists();
    }
    private void CheckIfCanvasExists()
    {
        // ��������� ������� ������� PlayerInventory � �������� �����
        CanvasDuplicateCheck existingCanvas = FindObjectOfType<CanvasDuplicateCheck>();
        if (existingCanvas != null && existingCanvas != this)
        {
            // ���� ������ ��� ���������� � �������� �����, ������� ���
            Destroy(this);
        }
    }
}
