<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField, Range(1.0f, 50.0f)]
    float damage = 10;
    public float Damage { get => damage; }

    [SerializeField, Range(500.0f, 2000.0f)]
    float moveSpeed = 700.0f;

    Rigidbody rigidbody;

    [SerializeField]
    float lifeTime = 5.0f;

    [SerializeField, Range(0.001f, 1.0f)]
    float moveInterval = 0.1f;

    IEnumerator move;

    [SerializeField, Range(1.0f, 50.0f)]
    float bulletForce = 20.0f;

    IEnumerator cleanBullets;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        cleanBullets = CleanBullets(lifeTime);
    }

    void Start()
    {
        Shoot();
    }

    void Shoot()
    {
        rigidbody.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        StartCoroutine(cleanBullets);
    }

    IEnumerator CleanBullets(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        GameManager.instance.Player.RemoveBullet(gameObject);
        Destroy(gameObject);
    }

    IEnumerator MoveBullet(float interval)
    {
        while(true)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(interval);
        }
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 5f;

    Rigidbody rb;

    [SerializeField]
    float bulletForce = 20f;

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
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.Health -= GameManager.instance.bulletDamage;

            if (enemy.Health <= 0 || GameManager.instance.instakillBuff == true)
            {
                enemy.Health = 0;
                enemy.Death();
            }
            else
                Debug.Log("El enemigo tiene: " + enemy.Health + " puntos de vida restantes");
        }
    }
}
>>>>>>> AnaBlanco2
