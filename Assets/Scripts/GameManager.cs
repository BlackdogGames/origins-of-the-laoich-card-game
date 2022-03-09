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
    PlayerStats _oppStats;

    CardStats _attackingCard;
    CardStats _defendingCard;

    void TurnLogic()
    {
        _oppStats = Opponent.GetComponent<PlayerStats>();
        _playerStats = Player.GetComponent<PlayerStats>();
        if (PlayersTurn)
        {
            //players turn
            //only access player cards
            foreach (var card in _oppStats.Cards)   // For each card in the players deck
            {
                card.GetComponent<DragDrop>().enabled = false;  // Disable Opponents Cards
            }
            foreach (var card in _playerStats.Cards)
            {
                card.GetComponent<DragDrop>().enabled = true;   // Enable Player Cards
            }
        }
        else
        {
            //opponents turn
            //only access opponent cards
            foreach (var card in _oppStats.Cards)   // For each card in the opponents deck
            {
                card.GetComponent<DragDrop>().enabled = true;   // Enable Opponents Cards
            }  
            foreach (var card in _playerStats.Cards)
            {
                card.GetComponent<DragDrop>().enabled = false;  // Disable Player Cards
            }
        }
        //PlayersTurn = !PlayersTurn; // Swap between Player/Opponent turns
    }

    public void CardAttackCard(GameObject attackingCard, GameObject defendingCard)
    {
        //check if input gameobjects are cards by checking if GetComponent<CardStats>() returns null

        // Subtracts the attacking cards attack damage from the defending cards health
        _attackingCard = attackingCard.GetComponent<CardStats>();
        _defendingCard = defendingCard.GetComponent<CardStats>();
        _defendingCard.Health -= _attackingCard.Attack;

        // Check for death of cards, delete from scene if dead
        if (_defendingCard.Health <= 0)
        {
            Destroy(_defendingCard);
        }
    }

    public void CardAttackPlayer(GameObject attackingCard, GameObject defendingPlayer)
    {
        // Subracts the cards attack damage from the player health
        _attackingCard = attackingCard.GetComponent<CardStats>();
        _playerStats = defendingPlayer.GetComponent<PlayerStats>();
        _playerStats.Health -= _attackingCard.Attack;

        // If player dies, reset scene
        if (_playerStats.Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ManaDecrease(GameObject placedCard, GameObject cardPlayer, PlayerStats _cardPlayerStats)
    {
        // Subtracts the played cards mana cost from the player
        CardStats placedCardStats = placedCard.GetComponent<CardStats>();
        _cardPlayerStats = cardPlayer.GetComponent<PlayerStats>();
        _cardPlayerStats.Mana -= placedCardStats.ManaCost;
    }

    public void ManaIncrease(GameObject player)
    {
        _playerStats = player.GetComponent<PlayerStats>();
        _playerStats.MaxMana += 1;    // Increment max mana by 1 every round.
        _playerStats.Mana = _playerStats.MaxMana;   // Set mana to the new max at the start of the new round
    }

    // Update is called once per frame
    void Update()
    {
        TurnLogic();
    }
}
