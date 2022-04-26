using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class DropdownScript : MonoBehaviour
{
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Decks/");
    public  TMP_Dropdown Dropper;

    private void Start()
    {
        List<string> m_DropOptions = new List<string>();

        FileInfo[] info = dir.GetFiles("*.txt");

        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name.ToString());
            m_DropOptions.Add(f.Name.ToString());
        }

        Dropper.AddOptions(m_DropOptions);
        //Dropper.
    }

}
