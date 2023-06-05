using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitch : MonoBehaviour
{
    // ���������� ��� ���������� ������ � ������ � ����������� � ���������
    public int selectedWeapon = 0;
    public TextMeshProUGUI ammoInfoText;

    // ������ �� ��������� ������ AmmoContainer
    public static AmmoContainer ammoContainer = new AmmoContainer();

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        // �������� ������� ������
        WeaponClass currentWeapon = FindObjectOfType<WeaponClass>();

        // ��������� ����� � ����������� � ���������
        ammoInfoText.text = currentWeapon.ammoInMag + " / " + WeaponClass.ammoContainer.ammoTypeValues[currentWeapon.m_type];

        // ��������� ���������� ��������� ������
        int previousSelectedWeapon = selectedWeapon;

        // ������������ ������ � ������� �������� ����
        if (Input.mouseScrollDelta.y < 0f)
        {
            selectedWeapon = (selectedWeapon >= transform.childCount - 1) ? 0 : selectedWeapon + 1;
        }
        if (Input.mouseScrollDelta.y > 0f)
        {
            selectedWeapon = (selectedWeapon <= 0) ? transform.childCount - 1 : selectedWeapon - 1;
        }

        // ���� ��������� ������ ����������, �������� ���
        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        // �������� ������� ������ � ������������ ���������
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}