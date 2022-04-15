using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider LoadingBar;

    public string[] LoadingTips = { "Tip 01", "Tip 02", "Tip 03", "Tip 04", "Tip 05", "Tip 06", "Tip 07", "Tip 08", "Tip 09", "Tip 10" };
    public TMP_Text LoadingText;

    private void Start()
    {
        string textToDisplay = RandomTip();

        LoadingText.text = textToDisplay;
    }

    private string RandomTip()
    {
        string randomTip = LoadingTips[Random.Range(0, LoadingTips.Length)];
        return randomTip;
    }

    public void LoadScene(string levelName)
    {
        StartCoroutine(LoadSceneAsynchronously(levelName));
    }

    IEnumerator LoadSceneAsynchronously(string levelName)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(levelName);
        LoadingScreen.SetActive(true);
        while(!Operation.isDone)
        {
            LoadingBar.value = Operation.progress;
            yield return null;
        }
    }
}
