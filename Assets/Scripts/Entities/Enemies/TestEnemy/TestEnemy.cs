using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemy : EnemyClass

{
    private AIPath aIPath;

    public Transform target;
    public float meleeDamage = 1f;
    public bool isDamaging = false;

    private TestEnemyStates _sm;
    private ParticleSystem _ps;
    private DropOnDeath _dod;
    private bool isAlreadyDead = false;
    private GameObject meleeCollider;

    private void Awake()
    {
        _sm = GetComponent<TestEnemyStates>();
        _ps = GetComponent<ParticleSystem>();
        _dod = GetComponent<DropOnDeath>();
        aIPath = GetComponent<AIPath>();
        meleeCollider = transform.GetChild(3).gameObject;
    }

    public override void TakeDamage(float damageAmount)
    {
        if (isAlreadyDead) // Проверяем, не мертв ли уже враг
            return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0f)
        {
            isAlreadyDead = true;
            _sm.ChangeState(_sm.waitingState);
            var main = _ps.main;
            main.loop = true;
            StartCoroutine(TimerOnDying()); // Запускаем Coroutine
        }
    }

    IEnumerator TimerOnDying()
    {
        yield return new WaitForSeconds(.7f);
        _dod.DropItemsOnDeath();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!(target == null)) //проверка на то что цель существует
        {
            aIPath.destination = target.position; //и если так то строить до неё путь
        }
        else
            return;

        Debug.Log(isDamaging);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !_sm.isAnNPC) //если сталкиваемся с игроком (нпс дамага не наносит)
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>(); //то достаем его скрипт
            player.TakeDamage(meleeDamage); //и нахуяриваем ему дамага
            isDamaging = true;
            if (player != null)
                meleeCollider.SetActive(true);
        }
    }

    
}
