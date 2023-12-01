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

    /* [Header("Animaciones")]*/
    [SerializeField] private Animator animator;
    Vector2 direction;
    // Sonido
    [SerializeField] private AudioClip danioSFX;
    [SerializeField] private AudioClip muerteSFX;

    public bool estaAtacando = false;

    [SerializeField]
    private float distanciaAtaqueCuerpoACuerpo = 1.5f;

    [SerializeField]
    private float fadeDuration = 0.5f;

    private SpriteRenderer enemyRenderer;
    private Material enemyMaterial;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyMaterial = Instantiate(enemyRenderer.material);
        enemyRenderer.material = enemyMaterial;
    }

    private void Update()
    {
        if (!estaAtacando && Vector3.Distance(transform.position, player.transform.position) < detectionDistance)
        {
            //recibo la dirección en la que se mueve
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;
            // Utiliza un umbral para determinar la dirección
            float angle = Vector3.SignedAngle(Vector3.up, moveDirection, Vector3.forward);
            if (angle >= -135f && angle < -45f) // Movimiento hacia la derecha
            {
                direction = Vector2.right;
                //GetComponent<SpriteRenderer>().sprite = rightSprite;
            }
            else if (angle >= -45f && angle < 45f) // Movimiento hacia arriba
            {
                direction = Vector2.up;
                //GetComponent<SpriteRenderer>().sprite = upSprite;
            }
            else if (angle >= 45f && angle < 135f) // Movimiento hacia la izquierda
            {
                direction = Vector2.left;
                //GetComponent<SpriteRenderer>().sprite = leftSprite;
            }
            else // Movimiento hacia abajo
            {
                direction = Vector2.down;
                //GetComponent<SpriteRenderer>().sprite = downSprite;
            }
            Animations();
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

            

            if (/*PJ.isDead == true &&*/ Vector3.Distance(transform.position, player.transform.position) < distanciaAtaqueCuerpoACuerpo)
            {
                if(!player.isDead)
                {
                    animator.SetBool("isMoving", false);
                    estaAtacando = true;

                    // Activa la animación de ataque según la dirección
                    if (direction == Vector2.right)
                        animator.SetBool("atacandoDer", true);
                    else if (direction == Vector2.up)
                        animator.SetBool("atacandoArriba", true);
                    else if (direction == Vector2.left)
                        animator.SetBool("atacandoIzq", true);
                    else if (direction == Vector2.down)
                        animator.SetBool("atacandoAbajo", true);

                    StartCoroutine(volverAPatrullar(0.6f));
                }
            }
        }
        else if (!estaAtacando)
        {
            Patrol();
        }
    }

    private IEnumerator volverAPatrullar(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetBool("isMoving", true);
        estaAtacando = false;

        animator.SetBool("atacandoDer", false);
        animator.SetBool("atacandoArriba", false);
        animator.SetBool("atacandoIzq", false);
        animator.SetBool("atacandoAbajo", false);
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
            direction = Vector2.right;
            //GetComponent<SpriteRenderer>().sprite = rightSprite;
        }
        else if (angle >= -45f && angle < 45f) // Movimiento hacia arriba
        {
            direction = Vector2.up;
            //GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        else if (angle >= 45f && angle < 135f) // Movimiento hacia la izquierda
        {
            direction = Vector2.left;
            //GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        else // Movimiento hacia abajo
        {
            direction = Vector2.down;
            //GetComponent<SpriteRenderer>().sprite = downSprite;
        }
        Animations();

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

    private void Animations()
    {
        if (direction.magnitude != 0)
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
            //animator.Play("run");
        }
        //else animator.Play("Idle");
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
            StartCoroutine(FadeAndDestroy());

            if (isBoss)
            {
                SpawnEnemies();
            }

            // Reproduce el sonido de muerte utilizando el SoundManager
            SoundManager.Instance.PlaySound(muerteSFX);

            // matamos al enemigo
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(SlowDownForSeconds(1f)); // Reduce la velocidad por 4 segundos
            SoundManager.Instance.PlaySound(danioSFX);
        }
    }

    private IEnumerator FadeAndDestroy()
    {
        float elapsedTime = 0f;
        Color startColor = enemyMaterial.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Transparente

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            enemyMaterial.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        // Asegurarse de que el objeto sea destruido después del fade
        Destroy(gameObject);
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
            Vector3 spawnPosition = transform.position;

            // Instancia dos enemigos a una distancia razonable entre sí
            Instantiate(enemyPrefab, spawnPosition + new Vector3(2f, 2f, 0f), Quaternion.identity);
            Instantiate(enemyPrefab, spawnPosition + new Vector3(-2f, -2f, 0f), Quaternion.identity);
        }
    }
}
