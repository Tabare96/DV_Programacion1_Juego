using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private Player player;

    private int index = 0;

    
    // Sprites de direcci�n
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
        Patrol();
       /* if (Vector3.Distance(transform.position, player.transform.position) < 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            Patrol();
        }*/
    }
    private void Patrol()
    {
        if (waypoints.Length == 0)
        {
            return;
        }

        Transform target = waypoints[index];

        //recibo la direcci�n en la que se mueve
        Vector3 moveDirection = (target.position - transform.position).normalized;
        // Utiliza un umbral para determinar la direcci�n
        float angle = Vector3.SignedAngle(Vector3.up, moveDirection, Vector3.forward);

        if (angle >= -45f && angle < 45f) // Movimiento hacia arriba
        {
            GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        else if (angle >= 45f && angle < 135f) // Movimiento hacia la izquierda
        {
            GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        else if (angle >= -135f && angle < -45f) // Movimiento hacia la derecha
        {
            GetComponent<SpriteRenderer>().sprite = rightSprite;
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
        Player player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            Debug.Log("Veo al jugador");
        }
    }

    public void TakeDamage(int damage) { }
}
