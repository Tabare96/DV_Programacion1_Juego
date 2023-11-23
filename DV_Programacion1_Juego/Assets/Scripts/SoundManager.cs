using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioClip keyGrabSound; // Sonido para cuando se agarra la llave
    [SerializeField] private AudioClip OpenDoorSound; // Sonido para cuando se abre la puerta
    [SerializeField] private AudioClip CloseDoorSound; // Sonido para cuando está cerrada la puerta


    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return instance;
        }
    }

    private AudioSource sfxAudioSource;
    private AudioSource ambientAudioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Componente AudioSource para efectos de sonido
        sfxAudioSource = gameObject.AddComponent<AudioSource>();

        // Componente AudioSource para sonidos ambientales
        ambientAudioSource = gameObject.AddComponent<AudioSource>();
        ambientAudioSource.loop = true; // Hacer que el sonido ambiental se reproduzca en bucle
    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlayAmbientSound(AudioClip clip)
    {
        ambientAudioSource.clip = clip;
        ambientAudioSource.Play();
    }
    public void SetAmbientVolume(float volume)
    {
        ambientAudioSource.volume = Mathf.Clamp01(volume);
    }

    public void StopAmbientSound()
    {
        ambientAudioSource.Stop();
    }
    
    public void PlayKeyGrabSound()
    {
        if (keyGrabSound != null)
        {
            sfxAudioSource.PlayOneShot(keyGrabSound);
        }
    }

    public void PlayOpenDoorSound()
    {
        if (OpenDoorSound != null)
        {
            sfxAudioSource.PlayOneShot(OpenDoorSound);
        }
    }
    public void PlayCloseDoorSound()
    {
        if (CloseDoorSound != null)
        {
            sfxAudioSource.PlayOneShot(CloseDoorSound);
        }
    }
}
