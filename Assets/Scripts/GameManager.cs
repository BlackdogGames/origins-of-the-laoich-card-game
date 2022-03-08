using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool PlayersTurn; // True if players turn, false if opponents turn

    public GameObject Player;
    public GameObject Opponent;

    PlayerStats _playerStats;
    OpponentStats _oppStats;

    public void TurnLogic()
    {
        _oppStats = Opponent.GetComponent<OpponentStats>();
        _playerStats = Player.GetComponent<PlayerStats>();
        if (PlayersTurn)
        {
            //players turn
            //only access player cards
            foreach (var card in _oppStats.OpponentCards)   // For each card in the players deck
            {
                card.GetComponent<DragDrop>().enabled = false;  // Disable Opponents Cards
            }
            foreach (var card in _playerStats.PlayerCards)
            {
                card.GetComponent<DragDrop>().enabled = true;   // Enable Player Cards
            }

        }
        else
        {
            //opponents turn
            //only access opponent cards
            foreach (var card in _oppStats.OpponentCards)   // For each card in the opponents deck
            {
                card.GetComponent<DragDrop>().enabled = true;   // Enable Opponents Cards
            }  
            foreach (var card in _playerStats.PlayerCards)
            {
                card.GetComponent<DragDrop>().enabled = false;  // Disable Player Cards
            }
        }
        //PlayersTurn = !PlayersTurn; // Swap between Player/Opponent turns
    }



    // Update is called once per frame
    void Update()
    {
        TurnLogic();
    }
}
