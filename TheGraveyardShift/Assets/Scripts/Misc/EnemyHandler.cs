using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHandler : MonoBehaviour
{
    public Transform enemies;

    public int initialCount;
    private int enemyCount;

    private bool tutorialSmallHerd = false;
    private bool clearedTutorialLevel = false;

    private Objectives objectives;

    // Start is called before the first frame update
    void Start()
    {
        initialCount = enemies.childCount;
        enemyCount = initialCount;

        objectives = GameObject.FindWithTag("Objectives").GetComponent<Objectives>();
    }

    public int GetCurrentEnemyCount()
    {
        enemyCount = enemies.childCount;
        return enemyCount;
    }

    public bool CheckIfDead(int required)
    {
        EnemyAI[] AIs = enemies.GetComponentsInChildren<EnemyAI>();
        int count = 0;
        for (int i = 0; i < AIs.Length; i++)
        {
            if (AIs[i].health <= 0)
            {
                count++;
            }
        }

        return count >= required;
    }

    private void Update()
    {
        GetCurrentEnemyCount();
        if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            bool killedEnough = CheckIfDead(3);
            if (!tutorialSmallHerd && killedEnough)
            {
                tutorialSmallHerd = true;
                objectives.CompleteObjective();
            }
            killedEnough = CheckIfDead(initialCount);
            if (!clearedTutorialLevel && killedEnough)
            {
                clearedTutorialLevel = true;
                objectives.CompleteObjective();
            }
        }
    }
}
