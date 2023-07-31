using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Массив префабов предметов

    // Метод, вызываемый при смерти существа
    public void DropItemsOnDeath()
    {
        int numItemsToDrop = 1; // Количество предметов, которые будут выпадать по умолчанию

        // Генерируем случайное число от 0 до 100
        int dropChance = Random.Range(0, 101);

        // Определяем количество предметов в зависимости от вероятности выпадения
        if (dropChance <= 65)
        {
            numItemsToDrop = 2;
        }
        else if (dropChance <= 40)
        {
            numItemsToDrop = 3;
        }
        else if (dropChance <= 20)
        {
            numItemsToDrop = 4;
        }
        else if (dropChance <= 10)
        {
            numItemsToDrop = 5;
        }
        else if (dropChance <= 2)
        {
            numItemsToDrop = 6;
        }

        // Проходим по массиву префабов предметов и создаем нужное количество предметов
        for (int i = 0; i < numItemsToDrop; i++)
        {
            // Выбираем случайный префаб из массива
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

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