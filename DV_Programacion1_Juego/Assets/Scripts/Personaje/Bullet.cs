using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifespan;

    [SerializeField]
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifespan);
    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.right));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(gameObject);
        
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        EnemigoADistancia demon = collision.gameObject.GetComponent<EnemigoADistancia>();

        if (enemigo != null)
        {
            //Debug.Log("Le pego a un enemigo");
            enemigo.TakeDamage(1);
        }

        if (demon != null)
        {
            demon.TakeDamage(1);
        }
    }
}
