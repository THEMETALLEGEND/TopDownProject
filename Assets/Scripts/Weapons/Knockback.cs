using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Transform gunGO;
    private Camera cam;
    Vector2 mousePos;
    void Start()
    {
        gunGO = GameObject.Find("GunController").transform;
        transform.rotation = gunGO.rotation;

        cam = FindObjectOfType<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //позиция мыши в мировых координатах
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            TestEnemyStates _sm = collision.GetComponent<TestEnemyStates>();
            _sm.ChangeState(_sm.stunnedState);
            Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>(); // получаем Rigidbody2D противника
            Vector2 playerToMouse = mousePos - (Vector2)transform.position; // вектор от игрока до курсора мыши

            enemyRB.AddForce(playerToMouse.normalized * 15, ForceMode2D.Impulse); // применяем силу отталкивания к противнику
        }
    }
}
