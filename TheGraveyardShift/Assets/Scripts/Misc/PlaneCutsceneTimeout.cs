using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneCutsceneTimeout : MonoBehaviour
{
    public float transitionTime = 60f;

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchScene());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pressed)
        {
            pressed = true;
            TransitionManagerClass.Transition("TutorialScene");
        }
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(transitionTime);
        TransitionManagerClass.Transition("TutorialScene");
    }
}
