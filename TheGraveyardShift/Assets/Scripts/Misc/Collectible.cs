using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Percent Spawn Chance")]
    [SerializeField]private int spawnChance;

    void Start()
    {
        this.gameObject.SetActive(false);
        if(this.gameObject.tag == "Ammo")
        {
            int rand = Random.Range(0, 100);
            if (rand < spawnChance)
                this.gameObject.SetActive(true);
        }
        else if(this.gameObject.tag == "Battery")
        {
            int rand = Random.Range(0, 100);
            if (rand < spawnChance)
                this.gameObject.SetActive(true);
        }
        else if(this.gameObject.tag == "Medkit")
        {
            int rand = Random.Range(0, 100);
            if (rand < spawnChance)
                this.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        
    }
}
