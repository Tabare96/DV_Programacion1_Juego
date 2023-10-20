using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public float lifespan;

    [SerializeField]
    private Rigidbody2D rb;

    //private string direccion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifespan);
    }


    private void Update()
    {
        /* Mal hecho
        if (Input.GetKeyDown(KeyCode.A))
        {
            direccion = "Left";
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direccion = "Right";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direccion = "Down";
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            direccion = "Up";
        }
        else
        {
            direccion = direccion;
        }
        */

    }
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.right));

        /* MAL
        if (direccion == "Up")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.up));
        }
        else if (direccion == "Right")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.right));
        }
        else if (direccion == "Left")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.left));
        }
        else if (direccion == "Down")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.down));
        }
       */
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            Debug.Log("Le pego a un enemigo");
            enemigo.TakeDamage(1);
            Destroy(gameObject);
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
