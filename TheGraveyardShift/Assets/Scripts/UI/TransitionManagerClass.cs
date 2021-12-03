using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManagerClass : MonoBehaviour
{
    private static TransitionFade transitionManager;

    private static void UpdateTransitionManager()
    {
        GameObject transitionGO = GameObject.FindWithTag("TransitionManager");
        if (transitionGO != null)
        {
            transitionManager = transitionGO.GetComponent<TransitionFade>();
        }
    }

    public static void Transition(string sceneName)
    {
        Time.timeScale = 1f;
        UpdateTransitionManager();
        if (transitionManager != null)
        {
            transitionManager.Transition(sceneName);
        }
    }
}
