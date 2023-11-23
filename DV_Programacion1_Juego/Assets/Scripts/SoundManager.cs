using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

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

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
