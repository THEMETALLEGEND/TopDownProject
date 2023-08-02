using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootMagnet : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("LootMagnet"))
        {
            other.TryGetComponent(out PickableClass pickableClass);
            pickableClass.SetTarget(transform.parent.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
