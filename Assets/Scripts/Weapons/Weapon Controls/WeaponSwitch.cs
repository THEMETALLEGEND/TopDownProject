using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitch : MonoBehaviour
{
    // Переменные для выбранного оружия и текста с информацией о боезапасе
    public int selectedWeapon = 0;
    public TextMeshProUGUI ammoInfoText;

    // Ссылка на экземпляр класса AmmoContainer
    public static AmmoContainer ammoContainer = new AmmoContainer();

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        // Получаем текущее оружие
        WeaponClass currentWeapon = FindObjectOfType<WeaponClass>();

        // Обновляем текст с информацией о боезапасе
        if (currentWeapon.ammoType.m_Type == AmmoType.Melee)
            ammoInfoText.text = "∞";
        else if (currentWeapon.ammoType.m_Type == AmmoType.Energy)
            ammoInfoText.text = WeaponClass.ammoContainer.ammoTypeValues[currentWeapon.m_type].ToString();
        else
            ammoInfoText.text = currentWeapon.ammoInMag + " / " + WeaponClass.ammoContainer.ammoTypeValues[currentWeapon.m_type];

        // Сохраняем предыдущее выбранное оружие
        int previousSelectedWeapon = selectedWeapon;

        // Переключение оружия с помощью колесика мыши
        if (Input.mouseScrollDelta.y < 0f)
        {
            selectedWeapon = (selectedWeapon >= transform.childCount - 1) ? 0 : selectedWeapon + 1;
        }
        if (Input.mouseScrollDelta.y > 0f)
        {
            selectedWeapon = (selectedWeapon <= 0) ? transform.childCount - 1 : selectedWeapon - 1;
        }

        // Если выбранное оружие изменилось, выбираем его
        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        // Выбираем текущее оружие и деактивируем остальные
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}