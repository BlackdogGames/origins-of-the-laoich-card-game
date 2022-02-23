using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCards : MonoBehaviour
{
    //
    public GameObject CardOne;
    public GameObject CardTwo;
    public GameObject PlayerArea;
    public GameObject OpponentArea;
    //

    List<GameObject> _cards = new List<GameObject>();


    //
    void Start()
    {
        _cards.Add(CardOne);
        _cards.Add(CardTwo);
    }
    //

    //
    public void OnClick()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject playerCard = Instantiate(_cards[Random.Range(0, _cards.Count)], new Vector3(0, 0, 0), Quaternion.identity); //  where a random card is instantiated from the list
            playerCard.transform.SetParent(PlayerArea.transform, false); // when object is instantiated, set it as child of PlayerArea

            GameObject enemyCard = Instantiate(_cards[Random.Range(0, _cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.transform.SetParent(OpponentArea.transform, false); // child of opponent area
        }
        

    }
    //

}
