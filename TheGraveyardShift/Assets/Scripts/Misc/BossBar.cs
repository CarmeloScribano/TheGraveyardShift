using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    [SerializeField]
    private GameObject slider;
    private bool visible;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            visible = !visible;
            if(visible)
            {
                slider.SetActive(true);
            }
            else
            {
                slider.SetActive(false);
            }
        }
    }
}
