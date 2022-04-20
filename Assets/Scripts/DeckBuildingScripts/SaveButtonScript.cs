using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButtonScript : MonoBehaviour
{
    public DeckManager DeckMang;
    
    public void onClick()
    {
         DeckMang.ExportDeck();
        // DeckMang.ExportDeckStats();
        //DeckMang.ExportDeckIDs();
    }
        
       
}
