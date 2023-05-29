using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    public TextMeshProUGUI ammoInfoText;
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponClass currentWeapon = FindObjectOfType<WeaponClass>();
        ammoInfoText.text = currentWeapon.currentAmmo + " / " + currentWeapon.playerInventory.TestWeaponAmmo;
        int previousSelectedWeapon = selectedWeapon;

        if (Input.mouseScrollDelta.y > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.mouseScrollDelta.y < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
