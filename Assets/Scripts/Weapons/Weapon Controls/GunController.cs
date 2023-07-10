using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera cam;

    private GameObject gunGO;
    private GameObject player;
    private Transform gunControllerTransform;
    private float gunAngle;

    private void Awake()
    {
        gunGO = GameObject.Find("GunController"); //находит объект контроллера пушек по имени
        gunControllerTransform = gunGO.transform; //получает трансформ контроллера пушек
        player = GameObject.Find("Player");
        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        gunControllerTransform.transform.position = player.transform.position; //перемещение guncontroller'а вместе с этим объектом ибо это не child

        SpriteRenderer currentWeaponSprite = gunControllerTransform.GetComponentInChildren<SpriteRenderer>(); //получает компонент спрайта текущего оружия
        Transform firePoint = gunControllerTransform.GetComponentInChildren<WeaponClass>().transform.Find("Firepoint"); //получает трансформ компонента Firepoint для текущего оружия
        var position = firePoint.localPosition; //получает локальную позицию Firepoint

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //позиция мыши в мировых координатах

        if (currentWeaponSprite.flipY && gunAngle < 90 && gunAngle > -90 || !currentWeaponSprite.flipY && (gunAngle > 90 || gunAngle < -90)) //если оружие повернуто в противоположную сторону от игрока
        {
            position.y *= -1;
            firePoint.localPosition = position;
            currentWeaponSprite.flipY = !currentWeaponSprite.flipY; //отражаем спрайт
        }
    }

    private void FixedUpdate()
    {
        GameObject playerGO = GameObject.Find("Player"); //находит объект игрока по имени
        Rigidbody2D playerRB = playerGO.GetComponent<Rigidbody2D>(); //получает Rigidbody2D игрока

        Vector2 lookDir = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - playerRB.position; //вектор направления от игрока до мыши в мировых координатах
        gunAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; //угол между направлением пушки и направлением курсора в градусах
        gunControllerTransform.rotation = Quaternion.Euler(0, 0, gunAngle); //поворот пушки на угол в направлении мыши
    }
}