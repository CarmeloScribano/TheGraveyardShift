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
        UpdateTransitionManager();
        if (transitionManager != null)
        {
            Time.timeScale = 1f;
            transitionManager.Transition(sceneName);
        }
    }
}
