using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispararPistola : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float bulletSpeed;

    public GameObject bulletPrefab;
    public Transform firePoint;


    // Update is called once per frame
    void Update()
    {
        ApuntarHaciaMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Disparar();
        }
    }

    void ApuntarHaciaMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );
        transform.up = direction;
    }

    void Disparar()
    {
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 fireDirection = mousePosition - firePoint.position;
        fireDirection.Normalize();

        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRigidbody = newBullet.GetComponent<Rigidbody2D>(); // Asegúrate de tener un Rigidbody2D en tu proyectil

        // Asignar velocidad al proyectil en la dirección del mouse
        bulletRigidbody.velocity = fireDirection * bulletSpeed;
    }
}
