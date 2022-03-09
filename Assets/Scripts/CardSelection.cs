using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelection : MonoBehaviour
{

    public GameObject ThisCard;
    public GameObject CardToAttack;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelection()
    {
        
        //if(_gameManager.PlayersTurn == true && ThisCard.pa)
       // ThisCard = gameObject;
        
    }
}
