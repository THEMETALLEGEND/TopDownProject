using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject model;
    private Animator animator;
    private Transform childModel;
    private CharacterController2D playerController;
    private Transform playerTransform;
    //[SerializeField]
    private Vector3 scale;
    [HideInInspector]public bool isFacingRight = true;
    public bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        childModel = this.transform.GetChild(0); //поиск ГО модели
        animator = childModel.GetComponent<Animator>(); //достаем аниматор из ГО модели
        playerController = GetComponent<CharacterController2D>(); //ищем скрипт типа названия скрипта
        playerTransform = GetComponent<Transform>();
        Debug.Log(childModel);
    }

   
    void Update()
    {
        Vector2 vel = playerController.playerVelocity; //переменная публичной переменной из скрипта charactercontroller2d
        if (vel.magnitude >= .1f)   //если velocity rigidbody внутри скрипта выше больше .1f 
        {
            animator.SetBool("IsMoving", true); //значит мы двигаемся и аниматор переключается на анимацию движения
            isMoving = true;
        }
        else
        {
            animator.SetBool("IsMoving", false); //если не двигаемся то idle
            isMoving = false;
        }
            

        if (Input.GetButtonDown("Sprint") && isMoving) //если зажат шифт И мы двигаемся то анимация бега
            animator.SetBool("IsRunning", true);
        else if (Input.GetButtonUp("Sprint") || !isMoving) //отпускаем кнопку или не двигаемся - выключаем бег
            animator.SetBool("IsRunning", false);

        //
        if (playerController.moveX <= -.1) //если input по горизонтали внутри скрипта выше (т.е. двигаемся влево)
        {
            if (isFacingRight)    //доп проверка булом защита от поворота каждый кадр
            {
                transform.localScale = Vector3.Scale(new Vector3(transform.localScale.x, transform.localScale.y), new Vector3(-1, 1));
                //умножение scale этого объекта на -1 и 1 через метод Vector3.Scale(), т.е. поворот объекта по x, чтобы scale зависел от реального размера ГО
                isFacingRight = false;
            }


        }
        else if (playerController.moveX >= .1)
        {
            if (!isFacingRight)
            {
                transform.localScale = Vector3.Scale(new Vector3(transform.localScale.x, transform.localScale.y), new Vector3(-1, 1));
                isFacingRight = true;
            }


        }

        
            
    }
}
