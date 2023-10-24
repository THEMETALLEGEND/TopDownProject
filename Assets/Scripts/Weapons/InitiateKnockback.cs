using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateKnockback : MonoBehaviour
{
    public GameObject circlePrefab;
    public float distance = 1.0f;
    public float duration = 0.5f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(SpawnCircle());
        }
    }

    private IEnumerator SpawnCircle()
    {
        GameObject circle = Instantiate(circlePrefab, transform.position + transform.right * distance, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(circle);
    }
}
