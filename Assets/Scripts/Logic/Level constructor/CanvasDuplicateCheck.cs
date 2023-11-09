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
        // Проверяем наличие объекта PlayerInventory в активной сцене
        CanvasDuplicateCheck existingCanvas = FindObjectOfType<CanvasDuplicateCheck>();
        if (existingCanvas != null && existingCanvas != this)
        {
            // Если объект уже существует в активной сцене, удаляем его
            Destroy(this);
        }
    }
}
