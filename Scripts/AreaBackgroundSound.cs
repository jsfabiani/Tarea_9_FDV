using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerController.onAreaChange += changeBackgroundSound;
    }


    void changeBackgroundSound(string gameObjectName)
    {
        Debug.Log("Activated");
        if ( gameObjectName == this.name)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
