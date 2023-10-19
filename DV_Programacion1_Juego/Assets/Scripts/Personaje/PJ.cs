using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJ : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private int health;

    private float horizontal;
    private float vertical;

    public Animator anim;

    Vector2 movement;

    public Transform shootingPoint;
    public Bullet prefab;
    public float bulletSpeed;

    private int maxMagAmmo = 7;
    
    private int magazineAmmo = 7;

   

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

  
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

        
        
        if (health <= 0)
        {
            Debug.Log("Me mori");
        }
    }

    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void shoot()
    {
        Bullet bullet = Instantiate(prefab, shootingPoint.position, shootingPoint.rotation);
        bullet.speed = bulletSpeed;
    }

    public float slowed(int slowing)
    {
        return movementSpeed /= slowing;
    }

    public float speedRegain(int slowing)
    {
        return movementSpeed *= slowing;
    }
}
