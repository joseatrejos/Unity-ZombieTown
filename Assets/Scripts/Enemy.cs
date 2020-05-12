using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Animator animator;

    [SerializeField]
    float health = 100;
    public float Health { get => health; set => health = value; }

    [SerializeField, Range(0.1f, 10f)]
    float moveSpeed = 3.5f;

    [SerializeField, Range(0f, 10f)]
    float minDistance = 5f;

    [SerializeField]
    int damage = 10;
    public int Damage { get => damage; }
    
    [SerializeField]
    int kill=1;
    
    [SerializeField]
    int killPoints = 5;
    
    public int KillPoints { get => killPoints; }

    NavMeshAgent navMeshAgent;

    void Start()
    {
        // Spawn
    }

    void Update()
    {
        if (!GameManager.instance.party.PartyDeath)
        {
            if (AttackRange)
            {
                if (!GameManager.instance.IsInChase)
                {
                    GameManager.instance.BeginChase();
                }
                navMeshAgent.destination = GameManager.instance.party.CurrentParty[0].transform.position;
                transform.LookAt(GameManager.instance.party.CurrentParty[0].transform);
            }
            else
            {
                navMeshAgent.destination = transform.position;

                if (GameManager.instance.IsInChase && distanceToPlayer <= minDistance && !GameManager.instance.IsInCombat)
                {
                    GameManager.instance.StartCombat();
                    //animator.SetLayerWeight(1, 1);
                }
                else if (OutOfAttackRange)
                {
                    GameManager.instance.EscapeCombatAndChase();
                    EndEnemyCombat();
                }
            }
        }
        else
        {
            GameManager.instance.GameOver();
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
        get => Vector3.Distance(this.transform.position, GameManager.instance.party.CurrentParty[0].transform.position);
    }

    public void EndEnemyCombat()
    {
        //animator.SetLayerWeight(1, 0);
        //animator.SetLayerWeight(0, 1);
    }

    public void Death()
    {
        // Insert death animation
        Debug.Log("El enemigo esta muerto");

        GameManager.instance.CountZombieKill(kill);

        navMeshAgent.isStopped = true;

        Destroy(gameObject.GetComponent<Collider>());

        // Replace seconds for the correct animations duration
        Destroy(gameObject, 2.0f);
    }
}
