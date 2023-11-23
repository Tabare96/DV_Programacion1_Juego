using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target; // El transform del jugador que seguirá la cámara
    public float smoothSpeed = 5f; // La velocidad de suavizado de la cámara

    [SerializeField] private AudioClip ambientSound; // Sonido ambiental
    [SerializeField] private float volume = 0.1f; // Volumen del sonido ambiental

    private void Start()
    {
        // Reproduce el sonido ambiental en bucle al inicio
        if (ambientSound != null)
        {
            SoundManager.Instance.PlayAmbientSound(ambientSound);
            // bajo el volumen
            SoundManager.Instance.SetAmbientVolume(volume);
        }
    }
    
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

