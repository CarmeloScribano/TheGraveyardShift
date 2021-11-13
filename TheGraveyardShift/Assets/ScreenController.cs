using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void RestartTutorialButton()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void AboutButton()
    {
        SceneManager.LoadScene("About");
    }
}
