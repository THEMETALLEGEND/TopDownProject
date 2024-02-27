using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityClass : MonoBehaviour    //класс любого живого существа
{
	//HEALTH SYSTEM
	public float currentHealth;
	public float maxHealth = 100f;

	void Start()
	{
		InitializeEntity();
	}

	protected virtual void InitializeEntity()
	{
		// Default initialization for entities
		currentHealth = maxHealth;
	}

	public void SetCurrentHealth(float health)
	{
		currentHealth = Mathf.Clamp(health, 0f, maxHealth);

		// Additional health-related checks or actions can be added here if needed in the future

		// For now, this method is straightforward and doesn't include a separate HealthCheck method
	}

	public virtual void TakeDamage(float damageAmount)
	{
		SetCurrentHealth(currentHealth - damageAmount);

		if (currentHealth <= 0f)
		{
			DestroyEntity();
		}
	}

	protected virtual void DestroyEntity()
	{
		Destroy(gameObject);
	}
}
