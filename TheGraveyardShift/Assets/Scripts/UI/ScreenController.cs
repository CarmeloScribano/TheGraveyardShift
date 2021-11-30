using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    public ScreenController settingsScreen;
    public ScreenController escapeScreen;
    public GameObject hud;

    public void Setup()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartTutorialButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TutorialScene");
    }

    public void RestartMainMapButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMap");
    }

    public void MainMapButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMap");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        TransitionManagerClass.Transition("MainMenu");
    }

    public void StartButton()
    {
        Time.timeScale = 1;
        TransitionManagerClass.Transition("PlaneScene");
    }

    public void AboutButton()
    {
        Time.timeScale = 1;
        TransitionManagerClass.Transition("About");
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        hud.SetActive(true);
    }

    public void SettingsButton()
    {
        gameObject.SetActive(false);
        settingsScreen.Setup();
    }

    public void PauseButton()
    {
        gameObject.SetActive(false);
        escapeScreen.Setup();
    }
}
