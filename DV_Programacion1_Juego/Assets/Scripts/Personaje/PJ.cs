using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PJ : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    
    [SerializeField]
    private float sprintSpeed;
    [SerializeField]
    private float walkSpeed;

    private Animator animator;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private int health;

    private bool isDead = false;

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

    public float maxStamina = 100f;
    [SerializeField] private float stamina = 100f;
    private bool staminaRegenerated = true;
    private bool sprinting = false;

    [SerializeField] private float staminaDrain = 10f;
    [SerializeField] private float staminaRegen = 5f;

    [SerializeField] private Image staminaUI;
    [SerializeField] private Image AmmoUI;


    [SerializeField] private AudioClip shootSFX; // Sonido disparo
    [SerializeField] private AudioClip deathSFX; // Sonido muerte

    [SerializeField] private List<AudioClip> walkSounds; // Lista de sonidos de pasos
    private AudioSource footstepAudioSource;

    private int isMovingID = Animator.StringToHash("isMoving");

    
    void Start()
    {
        footstepAudioSource = GetComponent<AudioSource>(); // Inicializamos el audio source

        animator = GetComponent<Animator>();
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

            AmmoUI.fillAmount = (float) magazineAmmo / maxMagAmmo;

            //Debug.Log(magazineAmmo + " balas en la pistola");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("Recargaste");

            if (magazineAmmo < maxMagAmmo)
            {
                magazineAmmo = maxMagAmmo;
            }

            AmmoUI.fillAmount = (float) magazineAmmo / maxMagAmmo;
        }

        // Sprint button

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = true;
            sprint(sprinting);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || stamina <= 0)
        {
            sprinting = false;
            movementSpeed = walkSpeed;
        }

        if (sprinting)
        {
            stamina -= staminaDrain * Time.deltaTime;
            staminaUI.fillAmount = (float)stamina / maxStamina;
        }
        else
        {
            if (stamina <= maxStamina - 0.01)
            {
                stamina += staminaRegen * Time.deltaTime;
                staminaUI.fillAmount = (float)stamina / maxStamina;

                if (stamina >= maxStamina)
                {
                    staminaRegenerated = true;
                    stamina = maxStamina;
                }
            }
        }
    }

    void FixedUpdate()
    {
        
        // REVISAR SI LO QUEREMOS
        if (isDead || movement.x != 0 && movement.y != 0)
        {
            return;
        }

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


    public void LateUpdate()
    {
        if (isDead)
        {
            return;
        }
        
        animator.SetBool(isMovingID, (movement.x != 0 || movement.y != 0) && !(movement.x != 0 && movement.y != 0));
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
    }

    public void sprint (bool sprinting)
    {
       if (staminaRegenerated)
            {
                sprinting = true;
                movementSpeed = sprintSpeed;
                

                if (stamina <= 0)
                {
                    staminaRegenerated = false;
                }
            }
        else
        {
            sprinting = false;
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
        health -= damage; 

        if (health <= 0)
        {
            //Debug.Log("Me mori");

            SoundManager.Instance.PlaySound(deathSFX);

            isDead = true;
            animator.SetBool(isMovingID, false);
            animator.SetBool("isDead", true);

            Invoke("ChangeToMenuMuerteScene", 2f);
        }
    }

    private void ChangeToMenuMuerteScene()
    {
        SceneManager.LoadScene("Menu_muerteTab");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            //Debug.Log("Le pego a un enemigo");
            TakeDamage(1);
        }
    }
}
