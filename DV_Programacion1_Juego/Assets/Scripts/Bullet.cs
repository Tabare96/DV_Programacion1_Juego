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

        //PJ PJ = collision.gameObject.GetComponent<Player>();
    }

    void FixedUpdate()
    {

        Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.right));

        /*
        if (PJ.direccion == "Up")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.up));
        }
        else if (PJ.direccion == "Right")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.right));
        }
        else if (PJ.direccion == "Left")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.left));
        }
        else if (PJ.direccion == "Down")
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, rb.rotation);
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * (Vector2)(rotation * Vector2.down));
        }

        */


    }

}
