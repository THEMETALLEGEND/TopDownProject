using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Transform gunGO;
    void Start()
    {
        gunGO = GameObject.Find("GunController").transform;
        transform.rotation = gunGO.rotation;
    }

    private void Update()
    {
    }
}
