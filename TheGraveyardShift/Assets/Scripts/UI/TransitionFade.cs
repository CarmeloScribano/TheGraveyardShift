using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionFade : MonoBehaviour
{
    private Image fadeImage;
    private Animator animator;

    public GameObject transitionPrefab;
    public float delay = 2f;

    public void Start()
    {
        StartCoroutine(Initialize());
    }

    public void Transition(string sceneName)
    {
        var tp = Instantiate(transitionPrefab);
        var image = GameObject.FindWithTag("FadeImage");

        fadeImage = image.GetComponent<Image>();
        animator = image.GetComponent<Animator>();

        StartCoroutine(Fading(sceneName));
    }

    public IEnumerator Fading(string sceneName)
    {
        animator.SetBool("Fade", true);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Initialize()
    {
        var tp = Instantiate(transitionPrefab);
        yield return new WaitForSeconds(1f);
        Destroy(tp);
    }
}
