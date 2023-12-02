using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyHolder : MonoBehaviour
{
    public event EventHandler OnKeysChanged;
    private List<Key.KeyType> keyList;

    private void Awake()
    {
        keyList = new List<Key.KeyType>();
    }
    
    public List<Key.KeyType> GetKeyList()
    {
        return keyList;
    }

    public void AddKey(Key.KeyType keyType)
    {
        //Debug.Log("Agarro la llave " + keyType);
        keyList.Add(keyType);
        SoundManager.Instance.PlayKeyGrabSound();
        //OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
        //OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Key key = collision.GetComponent<Key>();

        if (key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        KeyDoor keyDoor = collision.GetComponent<KeyDoor>();

        if (keyDoor != null)
        {
            if (ContainsKey(keyDoor.GetKeyType())) // Tiene la llave que habre la puerta
            {
                keyDoor.OpenDoor();
                RemoveKey(keyDoor.GetKeyType());
                SoundManager.Instance.PlayOpenDoorSound();
                // pregunto si el tag de la puerta es "PuertaBoss"
                if (keyDoor.gameObject.tag == "PuertaFinal")
                {
                    SceneManager.LoadScene("MenuTab");
                }
            }
            else
            {
                SoundManager.Instance.PlayCloseDoorSound();
                Debug.Log("No tienes la llave");
            }

        }
    }

}
