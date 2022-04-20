using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanButtonScript : MonoBehaviour
{

    public DeckManager DeckMang; // deck manager
    public GameObject Parent; // scrollview 

    public void onClick()
    {
        DeckMang.CleanDeck(); // clean up the list 
        
        foreach (Transform child in Parent.transform) { GameObject.Destroy(child.gameObject); } // deletes all the children in scrollview

    }


}
