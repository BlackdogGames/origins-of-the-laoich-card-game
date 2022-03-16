using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class DeckManager : MonoBehaviour
{
    // array/list of all the cards loaded in
    public List<Card> CardList = new List<Card>();
    // list of cards that would be added to the new deck (list to append to)

    // name of the deck
    // a txt/spreadsheet to save the newly built deck to

    // page counter/tracker

    public GameObject CardPrefab;
    public CardStats CardStats;


    void Start()
    {
        CardList = Resources.LoadAll("Cards").ToList().ConvertAll(item => (Card)item);
        // populate the grid with cards (6/8 on each 'page')
        Populate();
        // new deck list/counter empty
    }

    void Populate() // populate grid
    {
        GameObject _playerCard;

        for (int i = 0; i < CardList.Count(); i++)
        {
            _playerCard = Instantiate(CardPrefab, transform); //  where a card is instantiated from the list
            _playerCard.GetComponent<CardStats>().CardAsset = CardList[i];
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
