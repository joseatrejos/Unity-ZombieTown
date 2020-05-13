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

    // [SerializeField]
    // GameObject weapon;

    protected NavMeshAgent nav;

    protected bool invincible;

    void Start()
    {
        // WeaponVisibility(false);
        currentHealth = maxHealth;
    }

    void Awake()
    {
        // animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isNpc && alive == true)
        {
            GameplaySystem.Movement3D(transform, moveSpeed);
            moving = GameplaySystem.Axis3D != Vector3.zero;

            if (moving)
            {
                // Animations here
            }

            // Animator here

            // anim.SetBool("moving", moving);
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

            StartCoroutine(WaitForPassiveHeal());
        }
    }

    public void WeaponVisibility(bool visibility)
    {
        // weapon.SetActive(visibility);
    }

    void Shot()
    {
        // Bullet bullet = bulletGameObject.GetComponent<Bullet>();
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
        if (other.CompareTag("Collectable") && this.tag == "Player")
        {
            CollectableObject collectable = other.GetComponent<CollectableObject>();
            //GameManager.instance.AddPoints(collectable.Points);
            Debug.Log("ganastePuntos");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("NPC") && this.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (!p.HasParty)
            {
                GameManager.instance.party.JoinParty(p);
            }
        }
        else if (other.tag == "Medkit" && this.tag == "Player")
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
        else if (other.gameObject.tag == "Enemy" && this.tag == "Player")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (!invincible)
            {
                currentHealth -= enemy.Damage;

                // Aquí pon la animación de puntos de vida perdidos

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    GameManager.instance.party.KillLeader();
                    Death();
                }
                Debug.Log("Te quedan " + currentHealth + " puntos de vida");
                StartCoroutine(Damage(enemy));
                invincible = true;
            }
        }
        else if (other.tag == "Obstacle" && this.tag == "Player")
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
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle" && this.tag == "Player")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>(); ;
            obstacle.Unlock();
            obstacle.waitForHideMessage();
        }
    }
    IEnumerator WaitForPassiveHeal()
    {
        yield return new WaitForSeconds(10.0f);
        //Debug.Log("te curaste");

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

    IEnumerator Damage(Enemy enemy)
    {
        yield return new WaitForSeconds(3.0f);
        invincible = false;
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

    public void Death()
    {
        alive = false;
        // Insert death animation
        Debug.Log("El jugador está muerto");

        Destroy(gameObject.GetComponent<Collider>());

        // Replace seconds for the correct animations duration
        Destroy(gameObject, 2.0f);
    }

    float distanceToLeader
    {
        get => Vector3.Distance(this.transform.position, GameManager.instance.party.CurrentParty[npcLeader].transform.position);
    }
}