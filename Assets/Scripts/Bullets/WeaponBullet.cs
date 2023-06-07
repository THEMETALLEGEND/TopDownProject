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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hitbox") //если этот ГО сталкивается с ГО с тэгом hitbox
        {
            TestEnemy testEnemy = collision.gameObject.GetComponentInParent<TestEnemy>(); //создаем переменную типа TestEnemy (имя скрипта) и
                                                                                          //назначаем скрипт TestEnemy из родителя объекта, с которым столкнулись (ибо хитбокс всегда child)

            TestEnemyStates testEnemyStates = collision.gameObject.GetComponentInParent<TestEnemyStates>();
            if (testEnemyStates.isAlerted == false)
                testEnemyStates.isAlerted = true;
            testEnemy.TakeDamage(bulletDamageAmount); //вкладываем в назначенную переменную со скриптом метод TakeDamage и назначаем float
                                                      //if (testEnemyStates.isAnNPC)
                                                      //testEnemyStates.ChangeState(testEnemyStates.fleeingState);
        }
        Destroy(gameObject); //при любом столкновении дестрой себя
    }
}