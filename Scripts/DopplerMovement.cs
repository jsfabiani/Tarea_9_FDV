using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DopplerMovement : MonoBehaviour
{
    public float Speed = 50.0f;
    public Vector3 direction = new Vector3 (1.0f, 0.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("m"))
        {
            this.transform.Translate(direction * Speed * Time.deltaTime);
        }
    }
}
