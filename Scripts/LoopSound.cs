using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSound : MonoBehaviour
{
    public float Speed = 50.0f;
    public Vector3 direction = new Vector3 (1.0f, 0.0f, 0.0f);
    [SerializeField] AudioSource m_AudioSource;
    private bool m_Play = false;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.loop = true;
        m_AudioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && m_Play == false)
        {
            m_AudioSource.Play();
            m_Play = true;
        }
        if (Input.GetKeyDown("s") && m_Play == true)
        {
            m_AudioSource.Pause();
            m_Play = false;
        }
        if (m_Play == true)
        {
            transform.Translate(direction * Speed * Time.deltaTime);
        }
    }
}
