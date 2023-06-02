using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera cam;
    //----------------------------
    private Rigidbody2D rb;
    private GameObject weapon;
    private Transform gunControllerTransform;
    [HideInInspector] public GameObject gunGO;
    private GameObject firePointGO;
    private Transform firePoint;    //точка вылета пули
    private PlayerAnimation playerAnimation;
    private GameObject playerGO;
    private Transform playerTransform;
    private Rigidbody2D playerRB;
    private float gunAngle;  //угол пушки до мыши
    private float fireposition;

    Vector2 mousePos;

    private void Awake()
    {
        playerGO = GameObject.Find("Player"); //ГО игрока
        playerTransform = playerGO.GetComponent<Transform>(); //Трансформ игрока
        playerRB = playerGO.GetComponent<Rigidbody2D>();   //RigidBody игрока
        gunGO = GameObject.Find("GunController");   //ГО контроллера пушек
        gunControllerTransform = gunGO.transform;   //Трансформ контроллера пушек
        //Находим первое оружие в контроллере пушек
        playerAnimation = GetComponent<PlayerAnimation>();  //Скрипт анимации игрока

    }

    private void Update()
    {
        weapon = gunControllerTransform.GetComponentInChildren<WeaponClass>().gameObject;
        firePointGO = weapon.transform.GetChild(0).gameObject;
        firePoint = firePointGO.transform;
        fireposition = firePoint.position.y;
        WeaponClass currentWeapon = gunControllerTransform.GetComponentInChildren<WeaponClass>(); //Получаем ссылку на текущее оружие

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //позиция мышки в мире

        

        gunControllerTransform.transform.position = playerTransform.transform.position; //перемещение guncontroller'а вместе с этим объектом ибо это не child

        if (gunAngle <= 90 && gunAngle >= -90) //если мышь смотрит вправо на 180 градусов
        {
            //weaponSprite.flipX = false;
            weapon.GetComponent<SpriteRenderer>().flipY = false; //то переворачиваем спрайт
            fireposition *= -1;
            Debug.Log(firePointGO);
        }
        else// if (gunAngle > 90 && gunAngle < -90)
        {
            //weaponSprite.flipX = true;
            weapon.GetComponent<SpriteRenderer>().flipY = true;
            fireposition *= -1;
            Debug.Log(firePointGO);
        }
        //Debug.Log(firePoint.position.y);
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - playerRB.position; //вычитаем один вектор из другого и получаем вектора которые указывают друг на друга
        gunAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; //через метод Atan2 находим угол в градусах до цели (переводим в градусы через Rad2Deg)
        gunControllerTransform.rotation = Quaternion.Euler(0, 0, gunAngle); //поворот пушки через евлеровый угол по z
        //gunControllerTransform.position = player.position;
    }
}