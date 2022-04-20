using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportDeckButton : MonoBehaviour
{
    public DeckManager DeckMang;

    public void onClick()
    {
        DeckMang.ImportDeck();
    }
}
