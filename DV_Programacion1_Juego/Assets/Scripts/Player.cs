using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float turningSpeed;

    private Rigidbody2D myRigidbody;

    //private Cannon[] cannons;

    [SerializeField]
    private int maximumHealth;
    private int health;

    private float horizontal;
    private float vertical;
    //private Quaternion targetRotation;

    void Awake()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (myRigidbody == null)
        {
            Debug.LogError("No se encontro el componente Rigidbody2D");
        }
        //cannons = GetComponentsInChildren<Cannon>();
        //transform.position = new Vector3(0, 2, 0);
        health = maximumHealth;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        /*Vector3 direction = new Vector3(horizontal, vertical, 0);
        transform.position += movementSpeed * Time.deltaTime * direction.normalized;*/

        //transform.position += movementSpeed * Time.deltaTime * vertical * transform.up;
        //transform.Translate(movementSpeed * Time.deltaTime * vertical * Vector3.right);
        //transform.Rotate(Vector3.forward, -turningSpeed * Time.deltaTime * horizontal);
        //transform.rotation = transform.rotation * Quaternion.;

        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Vector3 direction = end - start;

        // Que la rotacion siga el mouse
        /*Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = Input.mousePosition - screenPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        /*if (Input.GetKey(KeyCode.D))
        {
            transform.position += movementSpeed * Time.deltaTime * Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += movementSpeed * Time.deltaTime * Vector3.left;
        }*/

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    for (int i = 0; i < cannons.Length; i++)
        //    {
        //        cannons[i].Shoot();
        //    }
        //}

        transform.rotation = Quaternion.Euler(0f, 0f, -turningSpeed * Time.deltaTime * horizontal) * transform.rotation;
    }

    void FixedUpdate()
    {
        /*Vector3 right = transform.right;
        right = transform.rotation * Vector3.right;
        Quaternion rotation = transform.rotation;
        rotation = Quaternion.Euler(0f, 0f, myRigidbody.rotation);*/

        //myRigidbody.MovePosition(myRigidbody.position + (Vector2)(movementSpeed * Time.fixedDeltaTime * vertical * transform.up));
        Quaternion rotation = Quaternion.Euler(0f, 0f, myRigidbody.rotation);
        myRigidbody.MovePosition(myRigidbody.position + movementSpeed * Time.fixedDeltaTime * vertical * (Vector2)(rotation * Vector2.up));
        //myRigidbody.MoveRotation(myRigidbody.rotation -turningSpeed * Time.fixedDeltaTime * horizontal);
        //myRigidbody.MoveRotation(Quaternion.RotateTowards(rotation, targetRotation, turningSpeed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage)
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
    }

    public bool TakeHealing(int healing)
    {
        if (health == maximumHealth)
        {
            return false;
        }

        health = Mathf.Min(health + healing, maximumHealth);
        /*health += healing;
        if (health > maximumHealth)
        {
            health = maximumHealth;
        }*/
        Debug.Log("Me sanaron " + health);
        return true;
    }
}
