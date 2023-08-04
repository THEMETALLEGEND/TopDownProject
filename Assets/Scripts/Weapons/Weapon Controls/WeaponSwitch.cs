using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitch : MonoBehaviour
{
    // Переменные для выбранного оружия и текста с информацией о боезапасе
    public int selectedWeapon = 0;
    public TextMeshProUGUI ammoInfoText;
    public PlayerInventory playerInventory;

    // Ссылка на экземпляр класса AmmoContainer
    public static AmmoContainer ammoContainer = new AmmoContainer();

    void Start()
    {
        SelectWeapon();
        playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
        ammoInfoText = GameObject.Find("AmmoCount").GetComponentInChildren<TextMeshProUGUI>();
    }


    void Update()
    {
        // Получаем текущее оружие
        WeaponClass currentWeapon = FindObjectOfType<WeaponClass>();

        // Как отображать пул патронов в интерфейсе
        if (currentWeapon.ammoType.m_Type == AmmoType.Melee)
            ammoInfoText.text = "∞";                                                                                             //"бесконечные" патроны если ближний бой
        else if (currentWeapon.ammoType.m_Type == AmmoType.Energy)
            ammoInfoText.text = WeaponClass.ammoContainer.ammoTypeValues[currentWeapon.m_type].ToString();                      //показываем патроны без магазина если энергитические патроны
        else
            ammoInfoText.text = currentWeapon.ammoInMag + " / " + WeaponClass.ammoContainer.ammoTypeValues[currentWeapon.m_type]; //все остальное показываем сначала магазин потом остальные патроны

        
        int previousSelectedWeapon = selectedWeapon;  // Сохраняем предыдущее выбранное оружие

        
        if (Input.mouseScrollDelta.y < 0f)  // Переключение оружия с помощью колесика мыши
        {
            selectedWeapon = GetNextInitializedWeaponIndex(selectedWeapon);
        }
        else if (Input.mouseScrollDelta.y > 0f)
        {
            selectedWeapon = GetPreviousInitializedWeaponIndex(selectedWeapon);
        }

        // Переключение оружия с помощью клавиш цифр
        for (int i = 0; i < PlayerInventory.weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (PlayerInventory.weapons[i] != null)
                    selectedWeapon = i;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)   // Если выбранное оружие изменилось, выбираем его
            SelectWeapon();
    }

    int GetNextInitializedWeaponIndex(int currentIndex)
    {
        int nextIndex = (currentIndex + 1) % PlayerInventory.weapons.Length;
        while (PlayerInventory.weapons[nextIndex] == null)
        {
            nextIndex = (nextIndex + 1) % PlayerInventory.weapons.Length;
            if (nextIndex == currentIndex)
            {
                // Если не найдено ни одного инициализированного оружия
                return currentIndex;
            }
        }
        return nextIndex;
    }

    int GetPreviousInitializedWeaponIndex(int currentIndex)
    {
        int previousIndex = (currentIndex - 1 + PlayerInventory.weapons.Length) % PlayerInventory.weapons.Length;
        while (PlayerInventory.weapons[previousIndex] == null)
        {
            previousIndex = (previousIndex - 1 + PlayerInventory.weapons.Length) % PlayerInventory.weapons.Length;
            if (previousIndex == currentIndex)
            {
                // Если не найдено ни одного инициализированного оружия
                return currentIndex;
            }
        }
        return previousIndex;
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