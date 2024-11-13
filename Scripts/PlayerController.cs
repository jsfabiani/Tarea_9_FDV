using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed = 5.0f, turnSpeed = 30.0f, collisionVolumeScale = 1.0f;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip impact;
    [SerializeField] private Rigidbody m_Rigidbody;
    private Vector3 m_Input, playerVelocity;
    private float collisionSpeed, collisionVolume;
    private bool m_IsPlaying = false, m_OnGround;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //this.transform.Translate(playerVelocity, Space.World);
        this.transform.LookAt(transform.position + m_Input * turnSpeed * Time.deltaTime);

        // Audio
        if(m_OnGround == true)
        {
            if (m_Input != Vector3.zero && m_IsPlaying == false)
            {
                m_AudioSource.Play();
                m_IsPlaying = true;
            }
            else
            {
                m_AudioSource.Pause();
                m_IsPlaying = false;
            }
        }
    }

    void FixedUpdate()
    {
        playerVelocity = m_Input * Speed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(transform.position + playerVelocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            collisionSpeed = playerVelocity.magnitude;            
            collisionVolume = Mathf.Clamp(collisionVolumeScale * collisionSpeed, 0f, 1f);
            Debug.Log(collisionVolume);
            m_AudioSource.PlayOneShot(impact, collisionVolume);
        }
        if(collision.gameObject.CompareTag("Floor"))
        {
            m_OnGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            m_OnGround = false;
        }
    }

}
