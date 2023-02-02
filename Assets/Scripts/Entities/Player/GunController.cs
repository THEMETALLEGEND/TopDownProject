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
    private Transform firePoint;
    private PlayerAnimation playerAnimation;
    private GameObject playerGO;
    private Transform playerTransform;
    private Rigidbody2D playerRB;
    private float gunAngle;

    Vector2 mousePos;

    private void Awake()
    {
        playerGO = GameObject.Find("Player");
        playerTransform = playerGO.GetComponent<Transform>();
        playerRB = playerGO.GetComponent<Rigidbody2D>();
        gunGO = GameObject.Find("GunController");
        gunControllerTransform = gunGO.transform;
        weapon = GameObject.Find("GunController/Weapon");
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        gunControllerTransform.transform.position = playerTransform.transform.position;

        if (gunAngle >= 0 && gunAngle <= 180)
        {

        }
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - playerRB.position; //вычитаем один вектор из другого и получаем вектора которые указывают друг на друга
        gunAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; //через метод Atan2 находим угол в градусах до цели (переводим в градусы через Rad2Deg)
        gunControllerTransform.rotation = Quaternion.Euler(0, 0, gunAngle); //поворот пушки через евлеровый угол по z
        //gunControllerTransform.position = player.position;
    }
}
