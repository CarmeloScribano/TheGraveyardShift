using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    [SerializeField]
    private GameObject slider;
    public bool invisible;
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
     
            if(invisible)
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
