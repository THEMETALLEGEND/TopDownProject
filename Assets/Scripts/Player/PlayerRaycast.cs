using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public LayerMask enemyLayer; // маска слоя "Enemies"
    public LayerMask obstaclesLayer;
    public float rayLength = 10f; // длина лучей
    public int rayCount = 48; // количество лучей
    public float angleStep = 7.5f; // шаг между углами лучей
    public bool showDebugGizmos; // показывать ли отладочные линии
    public bool raycastHitEnemy;

    private void Update()
    {
        // рисуем 48 лучей вокруг игрока
        raycastHitEnemy = false;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, enemyLayer | obstaclesLayer);

            // выбираем цвет для линии в зависимости от того, столкнулся ли луч с объектом
            Color lineColor = Color.green;

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                lineColor = Color.red;
                raycastHitEnemy = true;
            }

            if (hit.collider != null && hit.collider.CompareTag("Obstacles"))
            {
                lineColor = Color.blue;
            }

            // рисуем линию для луча
            if (showDebugGizmos)
                Debug.DrawLine(transform.position, hit.collider != null ? hit.point : transform.position + direction * rayLength, lineColor);
        }
    }
}
