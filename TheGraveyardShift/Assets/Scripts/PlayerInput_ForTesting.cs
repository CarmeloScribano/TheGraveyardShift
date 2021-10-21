using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput_ForTesting : MonoBehaviour
{

    public float speed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.back * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(Vector3.up, -rotationSpeed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(Vector3.up, rotationSpeed);
        }
    }
}
