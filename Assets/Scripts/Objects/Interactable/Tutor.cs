using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutor : MonoBehaviour
{
	private GameObject textObject;

	private void Awake()
	{
		textObject = transform.GetChild(1).gameObject;

		//�������: ��� ������ ��������� ���� ������-�� �������������. ������������� ������ ��������� �� �������� ����� �������
		//������� ������, �� �� ��������� ���������
		textObject.SetActive(true);
		textObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			textObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
			textObject.SetActive(false);
	}
}
