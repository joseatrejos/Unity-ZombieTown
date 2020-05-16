﻿using System.Collections;
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
        if (!isNpc && !GameManager.instance.Win)
        {
            GameplaySystem.Movement3D(transform, moveSpeed);
            moving = GameplaySystem.Axis3D != Vector3.zero;

            if (moving)
            {
                //animaciones
            }

            //animator

            //anim.SetBool("moving", moving);
            if (GameplaySystem.Axis3D != Vector3.zero && moveSpeed != 0)
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
                if (this.currentHealth < maxHealth)
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
            GameManager.instance.AddPoints(collectable.Points);
            Destroy(other.gameObject);
            if(GameManager.instance.Win)
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
                    StartCoroutine(GameManager.instance.ResetBuffs("damage"));
                    break;

                case "Instakill":
                    buffUse.ApplyInstaKillBuff();
                    StartCoroutine(GameManager.instance.ResetBuffs("instakill"));
                    break;

                case "DefenseBuff":
                    buffUse.ApplyDefenseBuff();
                    StartCoroutine(GameManager.instance.ResetBuffs("defense"));
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

                if (!invencible)
                {
                    currentHealth -= GameManager.instance.zombieDamage;

                    ScaleLife();

                    if (currentHealth > maxHealth)
                    {
                        currentHealth = maxHealth;
                        GameManager.instance.Life.transform.localScale = new Vector3(10.07844f, 10.07844f, 10.07844f);
                    }

                    // Aquí pon la animación de puntos de vida perdidos
                    if (currentHealth <= 0)
                    {
                        currentHealth = 0;
                        moveSpeed = 0;
                        GameManager.instance.party.KillLeader();
                        this.Death();
                    }

                    Debug.Log("Te quedan " + currentHealth + " puntos de vida");
                    StartCoroutine(Damage());
                    GameManager.instance.Invencible.SetActive(true);
                    invencible = true;
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

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(3.0f);
        GameManager.instance.Invencible.SetActive(false);
        invencible = false;
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
        // Insert death animation
        Debug.Log("El jugador esta muerto");

        Destroy(gameObject.GetComponent<Collider>());

        // Replace seconds for the correct animations duration
        Destroy(gameObject, 2.0f);

        if (GameManager.instance.party.CurrentParty.Count == 0)
        {
            // Make this a coroutine if you want to delay the GameOver-animation to be able to see your last leader's death
            GameManager.instance.gameOver.SetActive(true);
        }
    }

    public void ScaleLife()
    {
        GameManager.instance.Scale = (currentHealth * 100) / maxHealth;

        GameManager.instance.Life.transform.localScale = new Vector3(10.07844f * (GameManager.instance.Scale / 100f), 10.07844f * (GameManager.instance.Scale / 100f), 10.07844f * (GameManager.instance.Scale / 100f));
    }
}