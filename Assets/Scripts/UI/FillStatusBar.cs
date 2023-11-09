using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    private EntityClass entityHealth;
    private GameObject fillImageGameObject;
    private Image fillImage;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        fillImageGameObject = GameObject.Find("Fill Area/Fill");
        fillImage = fillImageGameObject.GetComponent<Image>();
    }
    private void Start()
    {
        //entityHealth = GameObject.Find("Player").GetComponent<EntityClass>();
    }

    public void FindPlayerHealth()
    {
        entityHealth = GameObject.Find("Player").GetComponent<EntityClass>();
    }


    private void Update()
    {
        float fillvalue = entityHealth.currentHealth / entityHealth.maxHealth;
        slider.value = fillvalue;
    }
}

