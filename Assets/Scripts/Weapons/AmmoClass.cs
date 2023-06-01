using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoClass
{
    public int currentAmmoOfType;
    public int maxAmmoOfType;

}
public class PistolAmmo : AmmoClass
{
    public PistolAmmo()
    {
        maxAmmoOfType = 120;
        currentAmmoOfType = maxAmmoOfType;
    }
}
public class RifleAmmo : AmmoClass
{
    public RifleAmmo()
    {
        maxAmmoOfType = 240;
        currentAmmoOfType = maxAmmoOfType;
    }
}
public class EnergyAmmo : AmmoClass
{
    public EnergyAmmo()
    {
        maxAmmoOfType = 400;
        currentAmmoOfType = maxAmmoOfType;
    }
}