using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public bool useSingleton = true;

    private static DDOL instance;

    private void Start()
    {
        if (useSingleton)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Дополнительные методы класса...
}