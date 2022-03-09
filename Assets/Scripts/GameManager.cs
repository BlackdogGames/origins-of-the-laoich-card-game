using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public bool PlayersTurn; // True if players turn, false if opponents turn

    public GameObject Player;
    public GameObject Opponent;

    PlayerStats _playerStats;
    OpponentStats _oppStats;

    CardStats _attackingCard;
    CardStats _defendingCard;

    public GameObject CardPrefab;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    // Used to randomise deck order
    System.Random _rng = new System.Random();

    void Start()
    {
        _oppStats = Opponent.GetComponent<OpponentStats>();
        _playerStats = Player.GetComponent<PlayerStats>();

        // Randomise player deck order
        _playerStats.Deck = _playerStats.Deck.OrderBy(card => _rng.Next()).ToList();
    }

    void TurnLogic()
    {
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
        _playerStats = Player.GetComponent<PlayerStats>();
        _playerStats.PlayerHealth -= _attackingCard.Attack;

        // If player dies, reset scene
        if (_playerStats.PlayerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ManaDecrease(GameObject placedCard, GameObject cardPlayer, GameObject opp)
    {
        // Subtracts the played cards mana cost from the player
        CardStats placedCardStats = placedCard.GetComponent<CardStats>();
        _playerStats = cardPlayer.GetComponent<PlayerStats>();
        _playerStats.PlayerMana -= placedCardStats.ManaCost;

        _oppStats = opp.GetComponent<OpponentStats>();
        _oppStats.OpponentMana -= placedCardStats.ManaCost;
    }

    public void ManaIncrease(GameObject player, GameObject opponent)
    {

        _playerStats = player.GetComponent<PlayerStats>();
        _playerStats.PlayerMaxMana += 1;    // Increment max mana by 1 every round.
        _playerStats.PlayerMana = _playerStats.PlayerMaxMana;   // Set mana to the new max at the start of the new round

        _oppStats = opponent.GetComponent<OpponentStats>();
        _oppStats.OpponentMaxMana += 1;
        _oppStats.OpponentMana = _oppStats.OpponentMaxMana;
    }

    public void DrawCard()
    {
        if (_playerStats.Deck.Count > 0)
        {
            GameObject _playerCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity); //  where a random card is instantiated from the list
            _playerCard.GetComponent<CardStats>().CardAsset = _playerStats.Deck[0]; 
            _playerStats.Deck.RemoveAt(0);
            _playerCard.transform.SetParent(PlayerArea.transform, false); // when object is instantiated, set it as child of PlayerArea
            _playerStats.PlayerCards.Add(_playerCard);
        }

        GameObject _enemyCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _enemyCard.transform.SetParent(OpponentArea.transform, false); // child of opponent area
        _oppStats.OpponentCards.Add(_enemyCard);
    }

    // Update is called once per frame
    void Update()
    {
        TurnLogic();
    }
}
