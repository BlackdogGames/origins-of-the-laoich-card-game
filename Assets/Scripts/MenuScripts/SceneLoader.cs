using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider LoadingBar;

    public void LoadScene(string LevelName)
    {
        StartCoroutine(LoadSceneAsynchronously(LevelName));
    }

    IEnumerator LoadSceneAsynchronously(string LevelName)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(LevelName);
        LoadingScreen.SetActive(true);
        while(!Operation.isDone)
        {
            LoadingBar.value = Operation.progress;
            yield return null;
        }
    }
}
