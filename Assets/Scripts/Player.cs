using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platform2DUtils.GameplaySystem;

public class Player : Character3D
{

    [SerializeField]
    Object bulletSrc;

    [SerializeField]
    List<GameObject> bullets;

    [SerializeField, Range(7, 40)]
    float bulletsLimit = 8f;

    protected bool invincible;

    void Start()
    {
        //WeaponVisibility(false);
        currentHealth = maxHealth;
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
            //anim.SetBool("moving", moving);
            if (GameplaySystem.Axis3D != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(GameplaySystem.Axis3D.normalized);
            }

            if(GameplaySystem.JumpBtn)
            {
                Shoot();
            }
        }
        else
        {
            StartCoroutine(WaitForPassiveHeal());
            base.Move();
        }
    }

    void Shoot()
    {
        // Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        if(CanCreateBullets)
        {
            GameObject bulletGameObject = (GameObject) Instantiate(bulletSrc, transform.position, transform.rotation);
            bullets.Add(bulletGameObject);
        }
    }

    bool CanCreateBullets
    {
        get => bullets.Count < bulletsLimit;
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

    public void WeaponVisibility(bool visibility)
    {
        //weapon.SetActive(visibility);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            CollectableObject collectable = other.GetComponent<CollectableObject>();
            //GameManager.instance.AddPoints(collectable.Points);

            Debug.Log("ganastePuntos");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("NPC"))
        {
            Player p = other.GetComponent<Player>();
            if (!p.HasParty)
            {
                GameManager.instance.party.JoinParty(p);
            }
        }
        if (other.tag == "Medkit")
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
    }

    IEnumerator WaitForPassiveHeal()
    {
        yield return new WaitForSeconds(6.0f);

        if (currentHealth < maxHealth)
        {
            currentHealth += cure;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && this.tag == "Player")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (!invincible)
            {
                currentHealth -= enemy.Damage;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }

                // Aquí pon la animación de puntos de vida perdidos
                Debug.Log("Te quedan " + currentHealth + " puntos de vida");

                StartCoroutine(ReceiveDamage(enemy));
                invincible = true;
            }
        }
    }

    IEnumerator ReceiveDamage(Enemy enemy)
    {
        yield return new WaitForSeconds(3.0f);
        invincible = false;
    }
}
