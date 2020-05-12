using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    float damage = 5f;

    [SerializeField]
    float lifeTime = 5f;

    Rigidbody rb;

    [SerializeField]
    float bulletForce = 20f;

    public float Damage { get => damage; }

    IEnumerator cleanBullet;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cleanBullet = CleanBullet(lifeTime);
    }

    void Start()
    {
        Shot();
    }

    void Shot()
    {
        rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        StartCoroutine(cleanBullet);
    }

    IEnumerator CleanBullet(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        GameManager.instance.party.CurrentParty[0].RemoveBullet(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Enemy enemy;
            enemy = other.gameObject.GetComponent<Enemy>();
            enemy.Health -= damage;
            Debug.Log("El enemigo tiene: " + enemy.Health + " puntos de vida restantes");
            
            if(enemy.Health <= 0)
            {
                enemy.Death();
            }                
        }
    }
}