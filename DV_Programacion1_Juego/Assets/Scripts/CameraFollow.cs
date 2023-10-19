using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El transform del jugador que seguirá la cámara
    public float smoothSpeed = 5f; // La velocidad de suavizado de la cámara

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}
