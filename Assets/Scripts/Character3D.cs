using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platform2DUtils.GameplaySystem;
using Cinemachine;
using System;
using UnityEngine.AI;

public class Character3D : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody rb;
    [SerializeField] protected float jumpForce = 7f;
    [SerializeField] public float moveSpeed = 2f;

    //******* Raycast *******
    [SerializeField] Color rayColor = Color.magenta;
    [SerializeField, Range(0.1f, 5f)] float rayDistance = 5f;
    [SerializeField] LayerMask groundLayer;
    //*******

    //********Jump**********
    [SerializeField] protected bool jump = false;
    protected float scale;
    //*******

    //****** Follow
    protected bool moving;

    [SerializeField] Player leader;

    [SerializeField] float minDistanceFollow;

    float dirX;
    float dirY;

    Vector3 npcDirection;

    protected Collider collider;

    [SerializeField] protected bool isLeader;
    public bool IsLeader { get => isLeader; set => isLeader = value; }

    [SerializeField] protected bool isNpc;
    public bool IsNpc { get => isNpc; set => isNpc = value; }

    [SerializeField] private bool hasParty;


    public bool HasParty { get => hasParty; set => hasParty = value; }

    [SerializeField]
    protected float maxHealth;

    [SerializeField]
    protected float currentHealth;

    [SerializeField]
    protected float cure;
    
    public NavMeshAgent navMeshAgent;
    //********

    void Update()
    {
        //anim.SetBool("moving", moving);
    }

    void Awake()
    {
        //anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    protected bool Grounding
    {
        get => Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
    }

    // Drawing raycast
    void OnDrawGizmosSelected()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);
    }

    public virtual void Move()
    {
        if (hasParty)
        {
            if (leader)
            {
                moving = Vector3.Distance(leader.transform.position, transform.position) > this.navMeshAgent.stoppingDistance;

                if (moving)
                {
                    // Esto es para decirle a la animación hacia donde tiene que moverse
                    npcDirection = leader.transform.position - transform.position;
                    npcDirection.Normalize();
                     if (GameplaySystem.Axis3D != Vector3.zero && moveSpeed != 0)
                    {
                    transform.rotation = Quaternion.LookRotation(GameplaySystem.Axis3D.normalized);
                    }
                    //aqui va el animator
                    //anim.SetFloat("moveX", npcDirection.x);
                    //anim.SetFloat("moveY", npcDirection.y);   
                }
                //animator
                anim.SetBool("moving", moving);
               
            }
        }
    }

    protected bool FlipSprite
    {
        //get => GameplaySystem.AxisTopdown.x < 0 ? true : GameplaySystem.AxisTopdown.x > 0 ? false : spr.flipX;
        get => true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Physics.IgnoreCollision(other.collider, collider);
        }
    }

    public Player Target
    {
        get => leader;
        set => leader = value;
    }
}
