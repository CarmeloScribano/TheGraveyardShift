using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneCutsceneTimeout : MonoBehaviour
{
    public GameObject[] explosions;
    public TraumaInducer inducer;

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
        yield return new WaitForSeconds(41f);
        for (int i = 0; i < explosions.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            explosions[i].SetActive(true);
            StartCoroutine(inducer.Explode());
        }
        yield return new WaitForSeconds(1f);
        TransitionManagerClass.Transition("TutorialScene");        
    }
}
