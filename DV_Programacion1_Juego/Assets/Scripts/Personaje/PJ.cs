using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PJ : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private int health;

    // Sprites de direcci�n
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

    
    [SerializeField] private AudioClip shootSFX; // Sonido disparo
    [SerializeField] private AudioClip deathSFX; // Sonido muerte

    [SerializeField] private List<AudioClip> walkSounds; // Lista de sonidos de pasos
    private AudioSource footstepAudioSource;

    
    void Start()
    {
        footstepAudioSource = GetComponent<AudioSource>(); // Inicializamos el audio source
    }

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
    }

    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + movement * movementSpeed * Time.fixedDeltaTime);

        // Reproduce un sonido de paso aleatorio cuando el personaje se mueve
        if (movement.magnitude > 0.1f && !footstepAudioSource.isPlaying)
        {
            // Elegir aleatoriamente un sonido de paso de la lista
            AudioClip randomWalkSound = walkSounds[Random.Range(0, walkSounds.Count)];

            // Reproducir el sonido de paso seleccionado
            footstepAudioSource.PlayOneShot(randomWalkSound);
        }
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

    public void TakeDamage(int damage)
    {
        health -= damage; // Reducir la vida por la cantidad de da�o recibido

        if (health <= 0)
        {
            Debug.Log("Me mori");
            
            SoundManager.Instance.PlaySound(deathSFX);

            Invoke("ChangeToMenuMuerteScene", 2f);
        }
    }

    private void ChangeToMenuMuerteScene()
    {
        SceneManager.LoadScene("Menu_muerte");
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
