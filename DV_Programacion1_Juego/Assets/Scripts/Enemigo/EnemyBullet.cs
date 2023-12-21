using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifespan = 3f;

    private Vector2 direction;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Mueve el proyectil en la direcci�n establecida
        rb.velocity = direction * speed;
    }

    // M�todo para configurar la direcci�n del proyectil
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Puedes agregar aqu� la l�gica de colisiones si es necesario

        Destroy(gameObject);

        // Aqu� tambi�n podr�as aplicar da�o al jugador u otros objetos si es necesario
    }
}
