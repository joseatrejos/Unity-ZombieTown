using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platform2DUtils.GameplaySystem;

public class Player : Character3D
{

   // [SerializeField]
   // GameObject weapon;

    void Start()
    {
       // WeaponVisibility(false);
    }

    void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isNpc)
        {
            GameplaySystem.Movement3D(transform, moveSpeed);
            moving = GameplaySystem.Axis3D != Vector3.zero;

            if (moving)
            {
                //animaciones
            }

            //animator
            //Sprite renderer
           
            //anim.SetBool("moving", moving);
               if(GameplaySystem.Axis3D != Vector3.zero)
                {
                  transform.rotation = Quaternion.LookRotation(GameplaySystem.Axis3D.normalized);
                }

        }
        else
        {
             StartCoroutine(WaitForPassiveHeal());
            base.Move();
        }
        //animator.SetFloat("move", Mathf.Abs(GameplaySystem.Axis3D.normalized.magnitude));
    }

    public void WeaponVisibility(bool visibility)
    {
       // weapon.SetActive(visibility);
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Collectable"))
        {
            CollectableObject collectable = other.GetComponent<CollectableObject>();
            //GameManager.instance.AddPoints(collectable.Points);
            Destroy(other.gameObject);
        }*/
        if(other.CompareTag("NPC"))
        {
             Player p = other.GetComponent<Player>();
            if(!p.HasParty)
            {
                GameManager.instance.party.JoinParty(p);
            }
        }
    }

    IEnumerator WaitForPassiveHeal()
    {
        yield return new WaitForSeconds(6.0f);

        if(currentHealth < maxHealth)
        {
            currentHealth += cure;
        }
    }
}
