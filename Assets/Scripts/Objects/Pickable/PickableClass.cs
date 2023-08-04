using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableClass : MonoBehaviour
{
    public bool magnetizable = false;
    public float magnetSpeed;
    Rigidbody2D rb;
    bool hasTarget;
    Vector3 targetPosition;
    //private PlayerInventory playerInventory;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void FixedUpdate()
    {
        if (magnetizable && hasTarget)
        {
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * magnetSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hitbox")
            CollisionCheck(other);
    }

    public virtual void CollisionCheck(Collider2D other)
    {
        if (GameObject.Find("Player Inventory").TryGetComponent(out PlayerInventory playerInventory) && other.name == "Hitbox")
        {
            // Компонент PlayerInventory найден в родителе объекта other
            playerInventory.StuffCollectedCount();
            Destroy(gameObject);
        }
        else
        {
            // Компонент PlayerInventory не найден
            Debug.Log("Не найден инвентарь");
        }
    }
}
