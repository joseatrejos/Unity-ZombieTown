using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Player : Character3D
{
    [SerializeField] 
    float moveSpeed = 0;

    Animator animator;
    public Animator Animator { get => animator; }

    [SerializeField]
    GameObject weapon;

    void Start()
    {
        WeaponVisibility(false);
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*
        if (!isNpc)
        {
            GameplaySystem.MovementTopdown(rb.transform, moveSpeed);
            moving = GameplaySystem.AxisTopdown != Vector2.zero;

            if (moving)
            {
                //animaciones
            }

            //animator
            //Sprite renderer
           
            anim.SetBool("moving", moving);

        }
        else
        {
            base.Move();
        }
        */

        transform.Translate(Axis.normalized.magnitude * Vector3.forward * moveSpeed * Time.deltaTime);
 
        if(Axis != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Axis.normalized);
        }

        animator.SetFloat("move", Mathf.Abs(Axis.normalized.magnitude));
    }

    Vector3 Axis
    {
        get => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    public void WeaponVisibility(bool visibility)
    {
        weapon.SetActive(visibility);
    }


    public bool IsLeader
    {
        get => isLeader;
        set => isLeader = value;
    }

    public bool IsNpc
    {
        get => isNpc;
        set => isNpc = value;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            CollectableObject collectable = other.GetComponent<CollectableObject>();
            //GameManager.instance.AddPoints(collectable.Points);
            Destroy(other.gameObject);
        }
    }
    
}
