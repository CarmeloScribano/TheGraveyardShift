using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    void Start()
    {
        if(this.gameObject.tag == "Ammo")
        {
            int rand = Random.Range(0, 100);
            if (rand < 40)
                this.gameObject.SetActive(false);
        }
        else if(this.gameObject.tag == "Battery")
        {
            int rand = Random.Range(0, 100);
            if (rand < 40)
                this.gameObject.SetActive(false);
        }
        else if(this.gameObject.tag == "Medkit")
        {
            int rand = Random.Range(0, 100);
            if (rand < 40)
                this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
