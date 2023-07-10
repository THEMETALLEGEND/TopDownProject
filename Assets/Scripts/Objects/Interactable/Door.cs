using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string keyId; // ID ключа, необходимого для открытия двери
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

        // Получаем компонент AstarPath с объекта A* в сцене
        AstarPath astarPath = GameObject.FindObjectOfType<AstarPath>();

        // Если нашли компонент AstarPath, вызываем метод Scan()
        if (astarPath != null)
        {
            astarPath.Scan();
        }

        DoorModelSwitch();
    }

    public void DoorModelCheckOnStart()
    {
        // Проверяем, есть ли дочерний объект Model_Open
        Transform childOpen = transform.Find("Model_Open");
        if (childOpen != null)
        {
            modelOpen = childOpen.gameObject;
        }

        // Проверяем, есть ли дочерний объект Model_Closed
        Transform childClosed = transform.Find("Model_Closed");
        if (childClosed != null)
        {
            modelClosed = childClosed.gameObject;
        }
    }

    public void DoorModelSwitch()
    {
        // Проверяем, существуют ли дочерние объекты
        if (modelOpen != null && modelClosed != null)
        {
            // Проверяем, включена ли дочь Model_Open
            if (modelOpen.activeSelf)
            {
                // Отключаем дочь Model_Open
                modelOpen.SetActive(false);
                // Включаем дочь Model_Closed
                modelClosed.SetActive(true);
            }
            else
            {
                // Отключаем дочь Model_Closed
                modelClosed.SetActive(false);
                // Включаем дочь Model_Open
                modelOpen.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Отсутствуют дочерние объекты Model_Open и Model_Closed.");
        }
    }
}