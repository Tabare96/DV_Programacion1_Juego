using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifespan = 3f;

    private Vector2 direction;
    [SerializeField]
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Mueve el proyectil en la dirección establecida
        rb.velocity = direction * speed;
    }

    // Método para configurar la dirección del proyectil
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        EnemigoADistancia demon = collision.gameObject.GetComponent<EnemigoADistancia>();

        if (demon != null)
        {

        }

        else
        {
            Destroy(gameObject);
        } 
            

        PJ player = collision.gameObject.GetComponent<PJ>();
        if (player != null)
        {
            player.TakeDamage(1);
        }
    }
}
