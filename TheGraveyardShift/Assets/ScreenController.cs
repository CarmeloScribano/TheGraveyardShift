using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    public ScreenController settingsScreen;
    public GameObject hud;

    public void Setup()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartTutorialButton()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void MainMapButton()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void MainMenuButton()
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

    public void ResumeButton()
    {
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
        settingsScreen.Setup();
    }
}
