using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private GameObject boss;
    private float maxHealth;
    private float currentHealth;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        maxHealth = boss.GetComponent<EnemyAI>().health;
        slider.maxValue = maxHealth;
        SetHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = boss.GetComponent<EnemyAI>().health;
        SetHealth(currentHealth);
    }

     void SetHealth(float health)
    {
        slider.value = health;
    }

}
