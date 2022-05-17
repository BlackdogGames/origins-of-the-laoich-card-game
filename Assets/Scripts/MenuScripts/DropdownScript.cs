using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class DropdownScript : MonoBehaviour
{
    private DirectoryInfo dir;
    public  TMP_Dropdown Dropper;

    public static string FileName;

    //New bits
    private FileInfo[] fileInfo;
    private List<string> m_DropOptions = new List<string>();

    private void Start()
    {

        dir = new DirectoryInfo(Application.persistentDataPath + "/Decks/");

        fileInfo = dir.GetFiles("*.txt");

        foreach (FileInfo f in fileInfo)
        {
            m_DropOptions.Add(f.Name.ToString());
            Debug.Log(f.Name.ToString());
        }

        foreach (var x in m_DropOptions)
        {
            string[] cards = File.ReadAllLines(Application.persistentDataPath + "/Decks/" + x);

            for (int i = 0; i < cards.Length; i++)
            {
                Debug.Log(cards[i]);
            }
        }

        //remove the last 4 characters of each option in dropoptions
        for (int i = 0; i < m_DropOptions.Count; i++)
        {
            m_DropOptions[i] = m_DropOptions[i].Remove(m_DropOptions[i].Length - 4);
        }

        Dropper.AddOptions(m_DropOptions);
        //Dropper.
    }

    private void Update()
    {

        FileName = Dropper.options[Dropper.value].text + ".txt";

        //Debug.Log(fileName);
    }

}