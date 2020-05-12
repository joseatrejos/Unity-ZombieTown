using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platform2DUtils.GameplaySystem;
using Cinemachine;
using System;

public class Character3D : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody rb;
    [SerializeField] protected float jumpForce = 7f;
    [SerializeField] protected float moveSpeed = 2f;

    //******* Raycast *******
    [SerializeField] Color rayColor = Color.magenta;
    [SerializeField, Range(0.1f, 5f)] float rayDistance = 5f;
    [SerializeField] LayerMask groundLayer;
    //*******

    //********Jump**********
    [SerializeField] protected bool jump = false;
    protected bool invencible = false;
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
    //********

    void Update()
    {
        //anim.SetBool("moving", moving);
    }

    void Awake()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
                moving = Vector3.Distance(leader.transform.position, transform.position) > minDistanceFollow;

                if (moving)
                {
                    // Esto es para decirle a la animación hacia donde tiene que moverse
                    npcDirection = leader.transform.position - transform.position;
                    npcDirection.Normalize();
                    transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, moveSpeed * Time.deltaTime);

                    if (npcDirection != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(npcDirection);
                    }

                    //aqui va el animator
                    //anim.SetFloat("moveX", npcDirection.x);
                    //anim.SetFloat("moveY", npcDirection.y);
                }
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
