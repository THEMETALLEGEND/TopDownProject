using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Массив префабов предметов

    // Метод, вызываемый при смерти существа
    public void DropItemsOnDeath()
    {
        // Проходим по массиву префабов предметов
        foreach (GameObject itemPrefab in itemPrefabs)
        {
            // Создаем экземпляр префаба на позиции существа, которое умерло
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            // Получаем компонент LootBounce из созданного предмета
            LootBounce lootBounce = newItem.GetComponent<LootBounce>();

            // Если компонент существует, включаем его
            if (lootBounce != null)
            {
                lootBounce.enabled = true;
            }
        }
    }
}