using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Percent Spawn Chance")]
    [SerializeField]private int spawnChance;
    Vector3 initialPos;

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
        else if(this.gameObject.tag == "Secret")
        {
            initialPos = transform.position;
        }
    }

    void Update()
    {
        if(this.gameObject.tag == "Secret")
        {
            float y = Mathf.PingPong(Time.time * 0.25f, 0.25f);
            transform.position = new Vector3(0, y, 0) + initialPos;
            transform.Rotate(0 ,45 * Time.deltaTime, 0);
        }
        
    }
}
