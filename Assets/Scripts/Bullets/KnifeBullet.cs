using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBullet : WeaponBullet
{
    public float knifeDamage = 10;
    void Start()
    {
        bulletDamageAmount = knifeDamage;
    }
    private void Update()
    {
        Destroy(gameObject, .1f); //если пуля не сталкивается ни с чем 3 секунды то она пропадет
    }
}
