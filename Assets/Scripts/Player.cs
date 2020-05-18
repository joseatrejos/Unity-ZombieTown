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
    float bulletsLimit = 5f;

    [SerializeField]
    float pushDamagedForce = 20f;

    [SerializeField]
    float buffsDuration = 12f;

    [SerializeField]
    float invencibilityCharacter = 3.0f;

    // [SerializeField]
    // GameObject weapon;

    void Start()
    {
        //WeaponVisibility(false);
        currentHealth = maxHealth;
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isNpc && !GameManager.instance.Win)
        {
            GameplaySystem.Movement3D(transform, moveSpeed);
            moving = GameplaySystem.Axis3D != Vector3.zero;

            anim.SetBool("moving", moving && !GameManager.instance.isShooting);

            if (GameplaySystem.Axis3D != Vector3.zero && moveSpeed != 0)
            {
                transform.rotation = Quaternion.LookRotation(GameplaySystem.Axis3D.normalized);
            }

            if (GameplaySystem.JumpBtn)
            {
                Shot();
            }
        }else
        {
            base.Move();
            for (int i = 1; GameManager.instance.party.CurrentParty.Count > i; i++)
            {
                if (this.currentHealth < maxHealth)
                {
                    StartCoroutine(WaitForPassiveHeal());
                }
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
        if (CanCreateBullets)
        {
            GameObject bulletGameObject = (GameObject)Instantiate(bulletSrc, transform.position, transform.rotation);
            bullets.Add(bulletGameObject);
        }
        anim.SetBool("shooting", true);
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
            GameManager.instance.AddPoints(collectable.Points);
            Destroy(other.gameObject);
            if (GameManager.instance.Win)
            {
                GameManager.instance.gameWin.SetActive(true);
            }
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
            ScaleLife();
            Destroy(other.gameObject);
        }
        else
        if (other.tag == "Obstacle" && this.tag == "Player")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>(); ;
            obstacle.ShowMessage();
        }
        else
        if (other.tag == "Buff" && this.tag == "Player")
        {
            CollectedBuff buffUse = other.GetComponent<CollectedBuff>();

            switch (buffUse.Buffs.name)
            {
                case "SpeedBuff":
                    buffUse.ApplySpeedBuff(this.GetComponent<Player>());
                    StartCoroutine(ResetSpeedBuff());
                    break;

                case "DamageBuff":
                    buffUse.ApplyDamageBuff();
                    StartCoroutine(GameManager.instance.ResetBuffs("damage", buffsDuration, this));
                    break;

                case "Instakill":
                    buffUse.ApplyInstaKillBuff();
                    StartCoroutine(GameManager.instance.ResetBuffs("instakill", buffsDuration, this));
                    break;

                case "DefenseBuff":
                    buffUse.ApplyDefenseBuff();
                    GameManager.instance.Invencibility = true;
                    StartCoroutine(GameManager.instance.ResetBuffs("defense", buffsDuration, this));
                    break;
            }

            Destroy(other.gameObject);
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
        // Reset rigidbody impulse to avoid perpetual rotation/movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (other.gameObject.tag == "Enemy")
        {
            if (this.tag == "Player")
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();

                if (!GameManager.instance.Invencibility)
                {
                    currentHealth -= GameManager.instance.zombieDamage;

                    GameManager.instance.Invencible.SetActive(true);
                    GameManager.instance.Invencibility = true;

                    ScaleLife();

                    // Aquí pon la animación de puntos de vida perdidos
                    if (currentHealth <= 0)
                    {
                        currentHealth = 0;
                        moveSpeed = 0;
                        GameManager.instance.party.KillLeader();
                        this.Death();
                    } else
                        StartCoroutine(ResetInvincibility());
                }
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        // Reset rigidbody impulse to avoid perpetual rotation/movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle" && this.tag == "Player")
        {
            Obstacle obstacle = other.GetComponent<Obstacle>(); ;
            if (obstacle.CanInteract)
            {
                obstacle.Unlock();
                obstacle.waitForHideMessage();
                if (Input.GetButton("Accept"))
                {
                    obstacle.CanInteract = false;
                }
            }
        }
    }

    IEnumerator WaitForPassiveHeal()
    {
        yield return new WaitForSeconds(1.0f);

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

    IEnumerator ResetInvincibility()
    {
        yield return new WaitForSeconds(invencibilityCharacter);
        GameManager.instance.Invencible.SetActive(false);
        GameManager.instance.Invencibility = false;
    }

    IEnumerator ResetSpeedBuff()
    {
        yield return new WaitForSeconds(12f);
        moveSpeed *= (2.0f / 3.0f);
        Debug.Log("Speed Buff reset");
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

    public void Death()
    {
        anim.SetBool("dead", true);
        Debug.Log("El jugador esta muerto");

        Destroy(gameObject.GetComponent<Collider>());

        GameManager.instance.Invencible.SetActive(false);
        GameManager.instance.Invencibility = false;

        if (GameManager.instance.party.CurrentParty.Count == 0)
        {
            // Make this a coroutine if you want to delay the GameOver-animation to be able to see your last leader's death
            GameManager.instance.gameOver.SetActive(true);
        }

        // Replace seconds for the correct animations duration
        Destroy(gameObject, 2.0f);
    }

    public void ScaleLife()
    {
        GameManager.instance.Scale = (currentHealth * 100) / maxHealth;

        GameManager.instance.Life.transform.localScale = new Vector3(GameManager.instance.LifeSize.x * (GameManager.instance.Scale / 100f), GameManager.instance.LifeSize.y * (GameManager.instance.Scale / 100f), GameManager.instance.LifeSize.z * (GameManager.instance.Scale / 100f));
    }
}