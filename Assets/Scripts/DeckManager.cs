using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro; 


public class DeckManager : MonoBehaviour
{

    public GameObject CardArea;
    public GameObject Player;

    public PlayerStats PlayerStatsInstance;
   
    public GameObject CardPrefab;
    public CardStats CardStats;

    public List<GameObject> CardList;
    public List<GameObject> CustomDeck;

    public TMP_InputField DeckNameInput;
    public TMP_InputField ImportInput;

    public GameObject ScrollviewParent; // the selection zone



    void Start()
    {
        PlayerStatsInstance = Player.GetComponent<PlayerStats>();
        PlayerStatsInstance.Deck = Resources.LoadAll("Cards").ToList().ConvertAll(item => (Card)item);

        // set the text field to interactable input
        DeckNameInput.interactable = true;
        ImportInput.interactable = true;

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

    

   
    void Update()
    {
        //
    }


    public void CleanDeck()
    {
        CustomDeck.Clear();
    }

    // a function to export CustomDeck to a text file
    public void ExportDeck()
    {
        string path = "Assets/Resources/Decks/" + DeckNameInput.text + ".txt";
        System.IO.File.WriteAllLines(path, CustomDeck.Select(x => x.GetComponent<CardStats>().CardAsset.name).ToArray());
    }

    // a function to import a text file to CustomDeck
    public void ImportDeck()
    {
        string path = "Assets/Resources/Decks/" + ImportInput.text + ".txt";
      //  string path = "Assets/Resources/Decks/id test deck.txt";
        CustomDeck = System.IO.File.ReadAllLines(path).ToList().ConvertAll(item => (GameObject)Resources.Load("Cards/" + item));
        for (int i = 0; i != CustomDeck.Count; i++)
        {
            CustomDeck[i].transform.SetParent(ScrollviewParent.transform, false); // set the cards as children of the scroll view
            
        }
    }


    ////a function to export all Card stats in CustomDeck to a spreadsheet
    //public void ExportDeckStats()
    //{
    //    string path = "Assets/Resources/Decks/" + DeckNameInput.text + ".csv";
    //    System.IO.File.WriteAllLines(path, CustomDeck.Select(x => x.GetComponent<CardStats>().CardAsset.name + "," + x.GetComponent<CardStats>().CardAsset.ManaCost + "," + x.GetComponent<CardStats>().CardAsset.Attack + "," + x.GetComponent<CardStats>().CardAsset.Health).ToArray());
    //}

}
