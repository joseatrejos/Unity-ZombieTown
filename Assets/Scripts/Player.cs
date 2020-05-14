using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platform2DUtils.GameplaySystem;
using UnityEngine.AI;

public class Player : Character3D
{
    [SerializeField]
    Object bulletSrc;

    [SerializeField]
    List<GameObject> bullets;

    [SerializeField]
    float bulletsLimit = 5;

    [SerializeField]
    float pushDamagedForce = 20;

    // [SerializeField]
    // GameObject weapon;

    public NavMeshAgent navMeshAgent;


    void Start()
    {
        //WeaponVisibility(false);
        currentHealth = maxHealth;
    }

    void Awake()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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

            //anim.SetBool("moving", moving);
            if (GameplaySystem.Axis3D != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(GameplaySystem.Axis3D.normalized);
            }

            if (GameplaySystem.JumpBtn)
            {
                Shot();
            }
        }
        else
        {
            base.Move();
            for (int i = 1; GameManager.instance.party.CurrentParty.Count > i; i++)
            {
                
                StartCoroutine(WaitForPassiveHeal());
                GameManager.instance.party.CurrentParty[i].navMeshAgent.destination = GameManager.instance.party.CurrentParty[i - 1].transform.position;
            }
        }
    }

    public void WeaponVisibility(bool visibility)
    {
        //weapon.SetActive(visibility);
    }

    void Shot()
    {
        //Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        if (CanCreateBullets)
        {
            GameObject bulletGameObject = (GameObject)Instantiate(bulletSrc, transform.position, transform.rotation);
            bullets.Add(bulletGameObject);
        }
    }

    bool CanCreateBullets
    {
        get => bullets.Count < bulletsLimit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable" && this.tag == "Player")
        {
            CollectableObject collectable = other.GetComponent<CollectableObject>();
            //GameManager.instance.AddPoints(collectable.Points);
            Debug.Log("ganastePuntos");
            Destroy(other.gameObject);
        }
        else
        if (other.tag == "NPC" && this.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (!p.HasParty)
            {
                GameManager.instance.party.JoinParty(p);
            }
        }
        else
        if (other.tag == "Medkit" && this.tag == "Player") 
        {
            MedkitUse medkitUse = other.GetComponent<MedkitUse>();
            currentHealth += medkitUse.Use();
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            Debug.Log(currentHealth);
            Destroy(other.gameObject);
        }
        else
        if (other.tag == "Obstacle" && this.tag == "Player")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>(); ;
            obstacle.ShowMessage();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle" && this.tag == "Player")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>(); ;
            obstacle.HideMessage();
            obstacle.CanInteract = true;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //rb.AddForce(Vector3.back * Axis.x, ForceMode.Impulse);
            //transform.position = (other.transform.position - new Vector3(1f, 0f, 0f) );
            
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (this.tag == "Player")
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();

                if (!invencible)
                {
                    currentHealth -= enemy.Damage;
                    if (currentHealth > maxHealth)
                    {
                        currentHealth = maxHealth;
                    }

                    // Aquí pon la animación de puntos de vida perdidos

                    if (currentHealth <= 0)
                    {
                        currentHealth = 0;
                        GameManager.instance.party.KillLeader();
                        this.Death();
                    }
                    Debug.Log("Te quedan " + currentHealth + " puntos de vida");
                    StartCoroutine(Damage());
                    invencible = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle" && this.tag == "Player")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>(); ;
            if(obstacle.CanInteract)
            {
                obstacle.Unlock();
                obstacle.waitForHideMessage();
                if(Input.GetButton("Acept"))
                {
                    obstacle.CanInteract = false;
                }
            }
        }
    }
    IEnumerator WaitForPassiveHeal()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("te curaste");

        if (currentHealth < maxHealth)
        {
            if (currentHealth + cure > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += cure;
            }
        }

    }


    IEnumerator Damage()
    {
        yield return new WaitForSeconds(3.0f);
        invencible = false;
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

    public void Death()
    {
        // Insert death animation
        Debug.Log("El jugador esta muerto");

        Destroy(gameObject.GetComponent<Collider>());

        // Replace seconds for the correct animations duration
        Destroy(gameObject, 2.0f);
    }
    
}