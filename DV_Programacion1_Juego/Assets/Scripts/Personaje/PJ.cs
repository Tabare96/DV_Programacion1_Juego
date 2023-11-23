using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PJ : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private int health;

    // Sprites de dirección
    [SerializeField]
    private Sprite upSprite;
    [SerializeField]
    private Sprite downSprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField]
    private Sprite rightSprite;

    private float horizontal;
    private float vertical;

    public Animator anim;

    Vector2 movement;

    public Transform shootingPointUp;
    public Transform shootingPointRight;
    public Transform shootingPointDown;
    public Transform shootingPointLeft;

    private Transform shootingPoint;


    public Bullet prefab;
    public float bulletSpeed;

    private int maxMagAmmo = 7;

    private int magazineAmmo = 7;

    static public string direccion;

    // Sonido disparo
    [SerializeField] private AudioClip shootSFX;
    

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.A))
        {
            direccion = "Left";
            GetComponent<SpriteRenderer>().sprite = leftSprite;
            shootingPoint = shootingPointLeft;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direccion = "Right";
            GetComponent<SpriteRenderer>().sprite = rightSprite;
            shootingPoint = shootingPointRight;

        }
        else if (Input.GetKey(KeyCode.S))
        {
            direccion = "Down";
            GetComponent<SpriteRenderer>().sprite = downSprite;
            shootingPoint = shootingPointDown;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direccion = "Up";
            GetComponent<SpriteRenderer>().sprite = upSprite;
            shootingPoint = shootingPointUp;
        }

        /*Animacion

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
        */

        if (Input.GetKeyDown(KeyCode.Space) && magazineAmmo > 0)
        {
            shoot();
            magazineAmmo -= 1;

            Debug.Log(magazineAmmo + " balas en la pistola");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Recargaste");

            if (magazineAmmo == 0)
            {
                magazineAmmo = maxMagAmmo;
            }
            if (magazineAmmo < 0)
            {
                magazineAmmo = maxMagAmmo + 1;
            }
        }



        /* if (health <= 0)
         {
             Debug.Log("Me mori");
         }*/
    }

    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void shoot()
    {
        Bullet bullet = Instantiate(prefab, shootingPoint.position, shootingPoint.rotation);
        bullet.speed = bulletSpeed;
        // sonido
        SoundManager.Instance.PlaySound(shootSFX);
    }

    public float slowed(float slowing)
    {
        return movementSpeed /= slowing;
    }

    public float speedRegain(float slowing)
    {
        return movementSpeed *= slowing;
    }

    /*public void TakeDamage(int damage)
    {
        // Reproducir sonido de daño.

        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Me mori");
        }
        else
        {
            Debug.Log("Me lastimaron " + health);
        }
    }*/

    public void TakeDamage(int damage)
    {
        health -= damage; // Reducir la vida por la cantidad de daño recibido

        if (health <= 0)
        {
            Debug.Log("Me mori");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            Debug.Log("Le pego a un enemigo");
            TakeDamage(1);
        }
    }
}
