using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public GameObject[] itemPrefabs; // ������ �������� ���������

    // �����, ���������� ��� ������ ��������
    public void DropItemsOnDeath()
    {
        // �������� �� ������� �������� ���������
        foreach (GameObject itemPrefab in itemPrefabs)
        {
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