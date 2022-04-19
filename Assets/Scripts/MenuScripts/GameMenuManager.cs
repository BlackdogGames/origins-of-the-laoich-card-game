using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public GameObject SettingsPanel;

    public static bool GamePaused = false;

    private void Start()
    {
        // Calls AudioManager to PLay a requested Sound
        AudioManager.Instance.Play("GameMusic");
    }

    private void Update()
    {
        // Calls AudioManager to Stop playing a Sound
        AudioManager.Instance.Stop("MasterMusic");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void Resume()
    {
        SettingsPanel.SetActive(false);
        GamePaused = false;
    }
    private void Pause()
    {
        SettingsPanel.SetActive(true);
        GamePaused = true;
    }

}
