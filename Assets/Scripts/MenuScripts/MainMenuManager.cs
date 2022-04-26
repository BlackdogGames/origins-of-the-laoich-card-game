using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenuManager : MonoBehaviour
{
   // public GameObject DeckSelection;

   // private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Decks");
    

    private void Start()
    {
        // Calls AudioManager to PLay a requested Sound
        AudioManager.Instance.Play("MasterMusic");
       // DeckSelection = GameObject.Find("Dropdown");

        //List<string> m_DropOptions = new List<string>();

        //FileInfo[] info = dir.GetFiles("*.*");
        //foreach (FileInfo f in info)
        //{
        //    //Debug.Log(f.ToString());
        //    m_DropOptions.Add(f.ToString());
        //}
        //DeckSelection.GetComponent<Dropdown>().AddOptions(m_DropOptions);

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
