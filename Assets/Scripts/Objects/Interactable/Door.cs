using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string keyId; // ID ключа, необходимого для открытия двери
    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        col.enabled = false;
        Debug.Log("opened");

        // Получаем компонент AstarPath с объекта A* в сцене
        AstarPath astarPath = GameObject.FindObjectOfType<AstarPath>();

        // Если нашли компонент AstarPath, вызываем метод Scan()
        if (astarPath != null)
        {
            astarPath.Scan();
        }
    }
}