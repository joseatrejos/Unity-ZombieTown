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
    
    [SerializeField]
    float bulletsLimit = 5; 

    // [SerializeField]
    // GameObject weapon;

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
            if(GameplaySystem.Axis3D != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(GameplaySystem.Axis3D.normalized);
            }

            if(GameplaySystem.JumpBtn)
            {
                Shot();
            }
        }
        else
        {
            StartCoroutine(WaitForPassiveHeal());
            base.Move();
        }
    }

    public void WeaponVisibility(bool visibility)
    {
        //weapon.SetActive(visibility);
    }

     void Shot()
    {
        //Bullet bullet = bulletGameObject.GetComponent<Bullet>();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            CollectableObject collectable = other.GetComponent<CollectableObject>();
            //GameManager.instance.AddPoints(collectable.Points);
            Debug.Log("ganastePuntos");
            Destroy(other.gameObject);
        }else
        if(other.tag == "NPC")
        {
             Player p = other.GetComponent<Player>();
            if(!p.HasParty)
            {
                GameManager.instance.party.JoinParty(p);
            }
        }else
        if(other.tag == "Medkit")
        {
            MedkitUse medkitUse = other.GetComponent<MedkitUse>();
            currentHealth += medkitUse.Use();
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            Debug.Log(currentHealth);
            Destroy(other.gameObject);
        }else
        if(other.gameObject.tag == "Enemy" && this.tag == "Player")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if(!invincible)
            {               
                currentHealth -= enemy.Damage;
                if(currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                
                // Aquí pon la animación de puntos de vida perdidos

                if(currentHealth <= 0)
                {
                    currentHealth = 0;
                    GameManager.instance.party.KillLeader();
                    Death();
                }
                Debug.Log("Te quedan " + currentHealth + " puntos de vida");
                StartCoroutine(Damage(enemy));
                invincible = true;
            }            
        }else
        if(other.tag == "Obstacle")
        {
            Obstacle obstacle =  other.GetComponent<Obstacle>();;
            obstacle.ShowMessage();
        }
    }

 void OnTriggerExit(Collider other)
    {
          if(other.tag == "Obstacle")
        {
            Obstacle obstacle =  other.GetComponent<Obstacle>();;
            obstacle.HideMessage();
        }
    }

    void OnTriggerStay(Collider other)
    {
     if(other.tag == "Obstacle")
        {
            Obstacle obstacle =  other.GetComponent<Obstacle>();;
            obstacle.Unlock();
            obstacle.waitForHideMessage();
        }   
    }
    IEnumerator WaitForPassiveHeal()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("te curaste");

        if(currentHealth < maxHealth)
        {
            if(currentHealth + cure > maxHealth)
            {
                currentHealth = maxHealth;
            }else
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
        // Insert death animation
        Debug.Log("El jugador esta muerto");

        Destroy(gameObject.GetComponent<Collider>());

        // Replace seconds for the correct animations duration
        Destroy(gameObject, 2.0f);
    }
}