using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
	// Singleton pattern for PlayerInventory
	public static PlayerInventory Instance;

	// Player-related data
	[SerializeField] private float playerHealth;


	public int AmmoCount { get; private set; }
	public int StuffCollected { get; private set; }


	//---------OBJECTS--------------
	private List<Key> keys = new List<Key>(); //������ ������ � ���������


	public UnityEvent<PlayerInventory> OnStuffCollected;

	public static Transform[] weapons = new Transform[9];
	public int selectedWeapon = -1;

	//�������: ������� �������� ������� ������ � ������� ���������� ��� �������
	public bool hasPistol = false;
	[HideInInspector] public bool pickedPistol = false;
	public bool hasRifle = false;
	[HideInInspector] public bool pickedRifle = false;
	public bool hasShotgun = false;
	[HideInInspector] public bool pickedShotgun = false;

	private void Awake()
	{
		InventoryUI inventoryUI = GameObject.Find("StuffCollectedCountText").GetComponent<InventoryUI>();
		OnStuffCollected.AddListener(inventoryUI.UpdateStuffCollectedText);

		weapons[0] = GameObject.Find("Knife").transform;

		// Implementing the Singleton pattern
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Method to set the player's health
	public void SetPlayerHealth(float health)
	{
		playerHealth = health;
	}

	// Method to get the player's health
	public float GetPlayerHealth()
	{
		return playerHealth;
	}

	public void StuffCollectedCount()
	{
		StuffCollected++;
		OnStuffCollected.Invoke(this);
	}
	public void AddKey(Key key)
	{
		keys.Add(key);
	}
	public bool HasKey(string keyId)
	{
		return keys.Any(key => key.keyId == keyId);
	}
}
public class AmmoContainer
{
	public Dictionary<AmmoType, int> maxAmmoTypeValues = new Dictionary<AmmoType, int>();
	public Dictionary<AmmoType, int> ammoTypeValues = new Dictionary<AmmoType, int>();

	public AmmoContainer()
	{
		maxAmmoTypeValues.Add(AmmoType.Melee, 0);
		maxAmmoTypeValues.Add(AmmoType.Pistol, 120);
		maxAmmoTypeValues.Add(AmmoType.Rifle, 240);
		maxAmmoTypeValues.Add(AmmoType.Shells, 50);
		maxAmmoTypeValues.Add(AmmoType.Energy, 400);

		ammoTypeValues.Add(AmmoType.Melee, 0);
		ammoTypeValues.Add(AmmoType.Pistol, 24);
		ammoTypeValues.Add(AmmoType.Rifle, 60);
		ammoTypeValues.Add(AmmoType.Shells, 6);
		ammoTypeValues.Add(AmmoType.Energy, 100);
	}
}
