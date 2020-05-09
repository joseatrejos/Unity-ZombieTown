using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Animator animator;

    [SerializeField, Range(0.1f, 10f)]
    float moveSpeed = 3.5f;

    [SerializeField, Range(0f, 10f)]
    float minDistance = 5f;

    [SerializeField]
    int damage = 10;
    public int Damage { get => damage; }


    NavMeshAgent navMeshAgent;

    void Start()
    {
        // Spawn
    }

    void Update()
    {
        if(AttackRange)
        {
            // In case you prefer the combat to begin as soon as the enemy starts following you

            if(!GameManager.instance.IsInChase)
            {
                GameManager.instance.BeginChase();
            }
            // transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            navMeshAgent.destination = GameManager.instance.Player.transform.position;
            transform.LookAt(GameManager.instance.Player.transform);
        }
        else
        {
            navMeshAgent.destination = transform.position;

            if(GameManager.instance.IsInChase && distanceToPlayer <= minDistance && !GameManager.instance.IsInCombat)
            {
                GameManager.instance.StartCombat();
                //animator.SetLayerWeight(1, 1);
            }
            else if(OutOfAttackRange)
            {
                GameManager.instance.EscapeCombatAndChase();
                EndEnemyCombat();
            }
        }
    }

    void LateUpdate()
    {
        //animator.SetBool("attack", AttackRange);
    }

    void Awake()
    {
        //animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    bool OutOfAttackRange
    {
        get => GameManager.instance.IsInChase && distanceToPlayer > minDistance;
    }
    
    bool AttackRange
    {
        get => distanceToPlayer <= minDistance && distanceToPlayer > navMeshAgent.stoppingDistance;
    }

    float distanceToPlayer
    {
        get => Vector3.Distance(this.transform.position, GameManager.instance.Player.transform.position);
    }

    public void EndEnemyCombat()
    {
        //animator.SetLayerWeight(1, 0);
        //animator.SetLayerWeight(0, 1);
    }
}
