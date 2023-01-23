using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : MonoBehaviour

{
    public float bulletDamageAmount = 40f; //урон пули

    private void Update()
    {
        Destroy(gameObject, 3f); //если пуля не сталкивается ни с чем 3 секунды то она пропадет
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") //если этот ГО сталкивается с ГО с тэгом enemy
        {
            TestEnemy testEnemy = collision.gameObject.GetComponent<TestEnemy>(); //создаем переменную типа TestEnemy (имя скрипта) и
                                                                                  //назначаем скрипт TestEnemy из объекта, с которым столкнулись
            testEnemy.TakeDamage(bulletDamageAmount); //вкладываем в назначенную переменную со скриптом метод TakeDamage и назначаем float
            //Debug.Log("collision enter");
        }
        Destroy(gameObject); //при любом столкновении дестрой себя
    }
}
