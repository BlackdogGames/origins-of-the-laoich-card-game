using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        // Calls AudioManager to PLay a requested Sound
        AudioManager.Instance.Play("MasterMusic");
    }

    private void Update()
    {
        // Calls AudioManager to Stop playing a Sound
        AudioManager.Instance.Stop("StartMusic");
        AudioManager.Instance.Stop("GameMusic");
    }

    // toggle fullscreen and save it to PlayerPrefs
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Origins of the Laoich");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
