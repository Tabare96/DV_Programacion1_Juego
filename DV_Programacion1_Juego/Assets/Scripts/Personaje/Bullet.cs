using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifespan;

    private Rigidbody2D myRigidbody;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //Destroy(gameObject, lifespan);
    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, myRigidbody.rotation);
        myRigidbody.MovePosition(myRigidbody.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.up));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            Debug.Log("Le pego a un enemigo");
            enemigo.TakeDamage(1);
        }
        //Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        //if (collision.gameObject.CompareTag("Enemigo"))

        //{
        //    Debug.Log("Le pego a un enemigo");
        //   // enemigo.TakeDamage(1); // Llama al método TakeDamage del enemigo
        //}

        //Player player = collision.gameObject.GetComponent<Player>();
        //if (player != null)
        //{
        //    Debug.Log("Le pego");
        //    player.TakeDamage(1);
        //    Destroy(gameObject);
        //}
        //if (enemigo != null)
        //{
        //    Debug.Log("Le pego a un enemigo");
        //    enemigo.TakeDamage(1); // Llama al método TakeDamage del enemigo
        //}
    }
}