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
    private bool bossDead = false;

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

    public bool CheckDeadBoss()
    {
        GameObject boss = GameObject.FindWithTag("Boss");

        if (boss != null)
        {
            EnemyAI ai = boss.GetComponent<EnemyAI>();
            if (ai != null)
            {
                if (ai.health <= 0)
                {
                    return true;
                }
            }
        }

        return false;
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
        if (SceneManager.GetActiveScene().name == "TutorialMap")
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
        else if (SceneManager.GetActiveScene().name == "BossMap")
        {
            if (!bossDead && CheckDeadBoss())
            {
                bossDead = true;
                objectives.CompleteObjective();
                StartCoroutine(EndGame());
            }
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        TransitionManagerClass.Transition("Credits");
    }

}
