using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool PlayersTurn; // True if players turn, false if opponents turn

    public GameObject Player;
    public GameObject Opponent;

    PlayerStats _playerStats;
    OpponentStats _oppStats;

    CardStats _attackingCard;
    CardStats _defendingCard;

    void TurnLogic()
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

    public void CardAttackCard(GameObject AttackingCard, GameObject DefendingCard)
    {
        //check if input gameobjects are cards by checking if GetComponent<CardStats>() returns null

        // Subtracts the attacking cards attack damage from the defending cards health
        _attackingCard = AttackingCard.GetComponent<CardStats>();
        _defendingCard = DefendingCard.GetComponent<CardStats>();
        _defendingCard.Health -= _attackingCard.Attack;

        // Check for death of cards, delete from scene if dead
        if (_defendingCard.Health <= 0)
        {
            Destroy(_defendingCard);
        }
    }

    public void CardAttackPlayer(GameObject AttackingCard, GameObject DefendingPlayer)
    {
        // Subracts the cards attack damage from the player health
        _attackingCard = AttackingCard.GetComponent<CardStats>();
        _playerStats = Player.GetComponent<PlayerStats>();
        _playerStats.PlayerHealth -= _attackingCard.Attack;

        // If player dies, reset scene
        if (_playerStats.PlayerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ManaDecrease(GameObject PlacedCard, GameObject CardPlayer, GameObject Opp)
    {
       CardStats _placedCard = PlacedCard.GetComponent<CardStats>();
        _playerStats = CardPlayer.GetComponent<PlayerStats>();
        _playerStats.PlayerMana -= _placedCard.ManaCost;

        _oppStats = Opp.GetComponent<OpponentStats>();
        _oppStats.OpponentMana -= _placedCard.ManaCost;
    }

    public void ManaIncrease(GameObject Player, GameObject Opponent)
    {
        _playerStats = Player.GetComponent<PlayerStats>();
        _playerStats.PlayerMana += 1;

        _oppStats = Opponent.GetComponent<OpponentStats>();
        _oppStats.OpponentMana += 1;
    }


    // Update is called once per frame
    void Update()
    {
        TurnLogic();
    }
}
