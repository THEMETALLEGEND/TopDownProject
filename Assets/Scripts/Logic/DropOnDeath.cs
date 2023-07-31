using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public GameObject[] itemPrefabs; // ������ �������� ���������

    // �����, ���������� ��� ������ ��������
    public void DropItemsOnDeath()
    {
        int numItemsToDrop = 1; // ���������� ���������, ������� ����� �������� �� ���������

        // ���������� ��������� ����� �� 0 �� 100
        int dropChance = Random.Range(0, 101);

        // ���������� ���������� ��������� � ����������� �� ����������� ���������
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

        // �������� �� ������� �������� ��������� � ������� ������ ���������� ���������
        for (int i = 0; i < numItemsToDrop; i++)
        {
            // �������� ��������� ������ �� �������
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // ������� ��������� ������� �� ������� ��������, ������� ������
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            // �������� ��������� LootBounce �� ���������� ��������
            LootBounce lootBounce = newItem.GetComponent<LootBounce>();

            // ���� ��������� ����������, �������� ���
            if (lootBounce != null)
            {
                lootBounce.enabled = true;
            }
        }
    }
}