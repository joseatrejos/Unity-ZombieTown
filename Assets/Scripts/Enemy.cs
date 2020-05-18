using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Platform2DUtils.GameplaySystem;

public class Enemy : MonoBehaviour
{
    Animator animator;
    [SerializeField] float maxHealth = 100;

    [SerializeField] float health = 100;
    public float Health { get => health; set => health = value; }

    [SerializeField, Range(0f, 10f)]
    float minDistance = 10f;

    [SerializeField]
    int kill = 1;

    [SerializeField]
    int killPoints = 5;

    public int KillPoints { get => killPoints; }

    Rigidbody rb;
    NavMeshAgent navMeshAgent;
    bool active = false;
    bool isDead = false;

    void Start()
    {
        // Spawn
        health = maxHealth;
    }

    void LateUpdate()
    {
        animator.SetBool("attack", AttackRange);
        GameManager.instance.party.CurrentParty[0].Anim.SetBool("shooting", GameManager.instance.isShooting);
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // If your party's still alive but you haven't won
        if (!GameManager.instance.party.PartyDeath && !GameManager.instance.Win)
        {
            if (AttackRange)
            {
                this.GetComponent<Collider>().isTrigger = false;
                if (!GameManager.instance.IsInChase)
                {
                    GameManager.instance.BeginChase();
                }
                navMeshAgent.destination = GameManager.instance.party.CurrentParty[0].transform.position;
                animator.SetBool("moving", true);
                transform.LookAt(GameManager.instance.party.CurrentParty[0].transform);
                
                GameManager.instance.isShooting = true;

                if(!isDead)
                {
                    navMeshAgent.speed = GameManager.instance.enemySpeed;
                }
            }
            else
            {
                navMeshAgent.destination = transform.position;
                animator.SetBool("moving", false);

                if (GameManager.instance.IsInChase && distanceToPlayer <= minDistance && !GameManager.instance.IsInCombat)
                {
                    GameManager.instance.StartCombat();
                    //animator.SetLayerWeight(1, 1);
                }
                else if (OutOfAttackRange)
                {
                    GameManager.instance.EscapeCombatAndChase();
                    EndEnemyCombat();
                    GameManager.instance.isShooting = false;
                }
            }
        }
        else // If you won
        if (GameManager.instance.Win)
        {
            navMeshAgent.destination = transform.position;

            animator.SetBool("dead", true);

            Destroy(this.gameObject, 2.0f);
        }
        else
        {
            // If your party died, stay where you are
            navMeshAgent.speed = 0f;
            animator.SetBool("moving", true);
        }
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
        // animator.SetLayerWeight(1, 0);
        // animator.SetLayerWeight(0, 1);
    }

    public void Death()
    {
        // Insert death animation

        GameManager.instance.CountZombieKill(kill, killPoints);
        GameManager.instance.ChangeRound();

        isDead = true;
        animator.SetBool("dead", true);
        navMeshAgent.speed = 0f;
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(DelayedTeleport());
    }

    IEnumerator DelayedTeleport()
    {
        yield return new WaitForSeconds(2f);
        transform.position = ObjectPooler.Instance.transform.position + new Vector3(Random.Range(-4.0f, 4.0f), 0, Random.Range(-4.0f, 4.0f));
        health = maxHealth;
        gameObject.GetComponent<Collider>().enabled = true;
        navMeshAgent.speed = GameManager.instance.enemySpeed;
        animator.SetBool("dead", false);
        isDead = false;
    }

    void OnCollisionEnter(Collision other)
    {
        // Reset rigidbody impulse to avoid perpetual rotation/movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionExit(Collision other)
    {
        // Reset rigidbody impulse to avoid perpetual rotation/movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

}
