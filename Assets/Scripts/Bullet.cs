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
    float speed = 5f;
    [SerializeField]
    float lifeTime = 5f;

    Rigidbody rb;

    IEnumerator move;

    [SerializeField]
    float moveInterval = 0.1f;

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
        //move = MoveBullet(moveInterval);

        //StartCoroutine(move);
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
        GameManager.instance.Player.RemoveBullet(gameObject);
        Destroy(gameObject);
    }

    IEnumerator MoveBullet(float interval)
    {
        while(true)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            yield return new WaitForSeconds(interval);
        }
    }
}
