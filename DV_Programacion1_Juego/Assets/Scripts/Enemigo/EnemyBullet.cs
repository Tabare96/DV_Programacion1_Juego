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
        // Puedes agregar aquí la lógica de colisiones si es necesario

        Destroy(gameObject);

        // Aquí también podrías aplicar daño al jugador u otros objetos si es necesario
    }
}
