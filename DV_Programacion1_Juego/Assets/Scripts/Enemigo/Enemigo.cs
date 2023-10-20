using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField]
    private bool isBoss = false;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int vida = 3;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private PJ player;

    [SerializeField]
    private float detectionDistance = 2f; // Variable para la distancia de detección

    private int index = 0;


    // Sprites de dirección
    [SerializeField]
    private Sprite upSprite;
    [SerializeField]
    private Sprite downSprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField]
    private Sprite rightSprite;


    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        Transform target = waypoints[index];

        //recibo la dirección en la que se mueve
        Vector3 moveDirection = (target.position - transform.position).normalized;
        // Utiliza un umbral para determinar la dirección
        float angle = Vector3.SignedAngle(Vector3.up, moveDirection, Vector3.forward);

        if (angle >= -135f && angle < -45f) // Movimiento hacia la derecha
        {
            GetComponent<SpriteRenderer>().sprite = rightSprite;
        }
        else if (angle >= -45f && angle < 45f) // Movimiento hacia arriba
        {
            GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        else if (angle >= 45f && angle < 135f) // Movimiento hacia la izquierda
        {
            GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        else // Movimiento hacia abajo
        {
            GetComponent<SpriteRenderer>().sprite = downSprite;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            index++;
            if (index >= waypoints.Length)
            {
                index = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PJ player = collision.GetComponentInParent<PJ>();
        if (player != null)
        {
            Debug.Log("Veo al jugador");
        }
    }

    public void TakeDamage(int damage)
    {
        vida -= damage; // Reducir la vida por la cantidad de daño recibido

        if (vida <= 0)
        {
            if (isBoss)
            {
                SpawnEnemies();
            }
            // matamos al enemigo
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(SlowDownForSeconds(1f)); // Reduce la velocidad por 4 segundos
        }
    }

    private IEnumerator SlowDownForSeconds(float seconds)
    {
        movementSpeed /= 2f;
        yield return new WaitForSeconds(seconds);
        movementSpeed *= 2f;
    }

    private void SpawnEnemies()
    {
        if (isBoss)
        {
            // Asegúrate de que solo el Boss pueda instanciar enemigos
            Vector3 spawnPosition = transform.position;

            // Instancia dos enemigos a una distancia razonable entre sí
            Instantiate(enemyPrefab, spawnPosition + new Vector3(2f, 2f, 0f), Quaternion.identity);
            Instantiate(enemyPrefab, spawnPosition + new Vector3(-2f, -2f, 0f), Quaternion.identity);
        }
    }
}
