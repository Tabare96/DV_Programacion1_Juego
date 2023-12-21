using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoADistancia : MonoBehaviour
{
    [SerializeField]
    private int vida = 3;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private PJ player;

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


    [Header("Desvanecimiento")]
    [SerializeField]
    private float fadeDuration = 2f;
    [SerializeField]
    private SpriteRenderer enemyRenderer;
    private Material enemyMaterial;


    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyMaterial = enemyRenderer.material;
        if (Camera.main == null)
        {
            Debug.LogError("No camera tagged as MainCamera found in the scene!");
        }
    }

    private void Update()
    {
        // Obtén la posición del enemigo en las coordenadas de la vista de la cámara
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Verifica si el enemigo está dentro de la cámara (en pantalla)
        bool isEnemyVisible = (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1);

        if (!isEnemyVisible)
        {
            // Si no está en pantalla, patrulla normalmente
            Patrol();
        }
        else if (!estaAtacando && !player.isDead && !player.playingDead)
        {
            // Si está en pantalla y no está atacando, ataca al jugador
            //recibo la dirección en la que se mueve
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;
            // Utiliza un umbral para determinar la dirección
            float angle = Vector3.SignedAngle(Vector3.up, moveDirection, Vector3.forward);

            if (angle >= -135f && angle < -45f) // Movimiento hacia la derecha
            {
                direction = Vector2.right;
            }
            else if (angle >= -45f && angle < 45f) // Movimiento hacia arriba
            {
                direction = Vector2.up;
            }
            else if (angle >= 45f && angle < 135f) // Movimiento hacia la izquierda
            {
                direction = Vector2.left;
            }
            else // Movimiento hacia abajo
            {
                direction = Vector2.down;
            }

            Animations();
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

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

    public void TakeDamage(int damage)
    {
        vida -= damage; // Reducir la vida por la cantidad de daño recibido

        if (vida <= 0)
        {
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
}
