using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{
    public PlayerStats Player;
    public TMP_Text Text;
    public GameObject[] Images;
    public int DeckMax = 30;

    // Update is called once per frame
    void Update()
    {
        Text.text = Player.Deck.Count.ToString();

        //Disable images by deck count / 30
        for (int i = 0; i < Images.Length; i++)
        {
            int threshold = DeckMax / Images.Length;

            if (Player.Deck.Count > (threshold * i))
            {
                Images[i].SetActive(true);
            }
            else
            {
                Images[i].SetActive(false);
            }
        }
    }
}
