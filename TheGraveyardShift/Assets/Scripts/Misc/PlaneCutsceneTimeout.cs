using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneCutsceneTimeout : MonoBehaviour
{
    public float transitionTime = 60f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchScene());
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("TutorialScene");
    }
}
