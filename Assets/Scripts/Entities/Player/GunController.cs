using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera cam;
    public Rigidbody2D player;
    //----------------------------
    private Rigidbody2D rb;
    private GameObject weapon;
    private Transform gunControllerTransform;
    private Transform firePoint;

    Vector2 mousePos;

    private void Awake()
    {
        gunControllerTransform = transform.Find("GunController");
        weapon = GameObject.Find("GunController/Weapon");
    }


    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - player.position; //вычитаем один вектор из другого и получаем вектора которые указывают друг на друга
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; //через метод Atan2 находим угол в градусах до цели (переводим в градусы через Rad2Deg)
        gunControllerTransform.rotation = Quaternion.Euler(0, 0, angle); //поворот пушки через евлеровый угол по z
        //gunControllerTransform.position = player.position;
    }
}
