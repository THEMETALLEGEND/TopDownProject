using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyClass : EntityClass
{
	private AIPath _aIPath;

	public Transform Target;
	public float MeleeDamage = 1f;
	public bool IsDamaging = false;

	private TestEnemyStates _sm;
	private ParticleSystem _ps;
	private DropOnDeath _dod;
	private bool _isAlreadyDead = false;
	private GameObject _meleeCollider;
	[SerializeField] private int _value;
	private PlayerInventory _playerInventory;

	private void Awake()
	{
		_sm = GetComponent<TestEnemyStates>();
		_ps = GetComponent<ParticleSystem>();
		_dod = GetComponent<DropOnDeath>();
		_aIPath = GetComponent<AIPath>();
		if (_sm.isMelee)
			_meleeCollider = transform.GetChild(3).gameObject;
	}

	public override void TakeDamage(float damageAmount)
	{
		if (_isAlreadyDead) // Проверяем, не мертв ли уже враг
			return;

		currentHealth -= damageAmount;

		if (currentHealth <= 0f)
		{
			_isAlreadyDead = true;
			_sm.ChangeState(_sm.WaitingState);
			var main = _ps.main;
			main.loop = true;
			StartCoroutine(TimerOnDying()); // Запускаем Coroutine
		}
	}

	IEnumerator TimerOnDying()
	{
		yield return new WaitForSeconds(.7f);
		_dod.DropItemsOnDeath();
		_playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
		_playerInventory.StuffCollectedCount(_value);
		Destroy(gameObject);
	}

	private void Update()
	{
		if (!(Target == null)) //проверка на то что цель существует
		{
			_aIPath.destination = Target.position; //и если так то строить до неё путь
		}
		else
			return;

		Debug.Log(IsDamaging);
	}

	private void OnCollisionStay2D(Collision2D collision) //change to Enter
	{
		if (collision.gameObject.tag == "Player" && !_sm.isAnNPC) //если сталкиваемся с игроком (нпс дамага не наносит)
		{
			CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>(); //то достаем его скрипт
			player.TakeDamage(MeleeDamage); //и нахуяриваем ему дамага
			IsDamaging = true;

			if (_sm.isMelee)
				_meleeCollider.SetActive(true);
		}
	}
}