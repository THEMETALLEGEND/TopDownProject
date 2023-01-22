using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityClass : MonoBehaviour    //класс любого живого существа
{
    //HEALTH SYSTEM
    public float currentHealth;
    public float maxHealth = 100f;

    void Start() //на методе Start() не работает
    {
        currentHealth = maxHealth; //на запуске ГО даем ему максимум ХП
    }

    public void TakeDamage(float damageAmount)  //метод отвечающий за прием дамага, принимающий float
    {
        currentHealth -= damageAmount;  //при вызове метода отнимаем вложенный float из настоящего здоровья

        if (currentHealth <= 0f) //если ХП 0 то смерть и дестрой себя
        {
            Destroy(gameObject);
        }
    }
}
