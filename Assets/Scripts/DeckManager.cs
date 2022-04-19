using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class DeckManager : MonoBehaviour
{

    public GameObject CardArea;
    public GameObject Player;

    public PlayerStats PlayerStatsInstance;
   
    public GameObject CardPrefab;
    public CardStats CardStats;

    public List<GameObject> CardList;
    public List<GameObject> CustomDeck;


    void Start()
    {
        PlayerStatsInstance = Player.GetComponent<PlayerStats>();
        PlayerStatsInstance.Deck = Resources.LoadAll("Cards").ToList().ConvertAll(item => (Card)item);

        // populate the grid with cards 
        Populate();
        
    }

    void Populate() // populate grid
    {
        PlayerStats playerStats = Player.GetComponent<PlayerStats>();

       // GameObject playerCard;

        for (int i = 0; i != PlayerStatsInstance.Deck.Count; i++)
        {
            GameObject playerCard = Instantiate(CardPrefab, transform); //  where a card is instantiated from the list
            playerCard.transform.SetParent(CardArea.transform, false); // set the parent to the grid
            playerCard.GetComponent<CardStats>().CardAsset = PlayerStatsInstance.Deck[i]; // give the card it's stats
            
            CardList.Add(playerCard); // add all of the cards in a list to keep track
           
        }
    }

    void AddCardToDeck()
    {
        // check how many of the same card exist before adding to the deck
        // add a selected card to the deck


    }

   
    void Update()
    {
        //
    }

    void OnRightPageTurn()
    {
        // display the next set of cards (increment the card array)
    }

    void OnLeftPageTurn()
    {
        // display the previous set of cards (decrement the card array)
    }

    void SaveDeck()
    {
        // save the current deck to a file
    }

    void CleanDeck()
    {
        // clear the deck / selections
    }
}
