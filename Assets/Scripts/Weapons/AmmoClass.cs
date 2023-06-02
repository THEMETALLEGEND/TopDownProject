using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoClass
{
    public AmmoType m_Type;
    public int currentAmmoOfType;
    public int maxAmmoOfType;
    public AmmoContainer ammoContainer;

    public void Refresh()
    {
        currentAmmoOfType = ammoContainer.ammoTypeValues[m_Type];
    }
}

public class PistolAmmo : AmmoClass
{
    public PistolAmmo(AmmoContainer ammoContainer)
    {
        this.ammoContainer = ammoContainer;
        m_Type = AmmoType.Pistol;
        maxAmmoOfType = ammoContainer.maxAmmoTypeValues[m_Type];
        currentAmmoOfType = ammoContainer.ammoTypeValues[m_Type];
    }
}

public class RifleAmmo : AmmoClass
{
    public RifleAmmo(AmmoContainer ammoContainer)
    {
        this.ammoContainer = ammoContainer;
        m_Type = AmmoType.Rifle;
        maxAmmoOfType = ammoContainer.maxAmmoTypeValues[m_Type];
        currentAmmoOfType = ammoContainer.ammoTypeValues[m_Type];
    }
}

public class EnergyAmmo : AmmoClass
{
    public EnergyAmmo(AmmoContainer ammoContainer)
    {
        this.ammoContainer = ammoContainer;
        m_Type = AmmoType.Energy;
        maxAmmoOfType = ammoContainer.maxAmmoTypeValues[m_Type];
        currentAmmoOfType = ammoContainer.ammoTypeValues[m_Type];
    }
}