using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
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

    public TMP_Text CardCounter;



    void Start()
    {
        PlayerStatsInstance = Player.GetComponent<PlayerStats>();
        PlayerStatsInstance.Deck = Resources.LoadAll("DBCards").ToList().ConvertAll(item => (Card)item);

        // set the text field to interactable input
        DeckNameInput.interactable = true;
        ImportInput.interactable = true;

        // populate the grid with cards 
        Populate();

        //Do not ask why this is here, no idea either <3
        //Go away.
        GameObject.Find("Collection_Panel").SetActive(false);

        CardCounter.text = "0";

    }

    void Populate() // populate grid
    {
        PlayerStats playerStats = Player.GetComponent<PlayerStats>();

       // GameObject playerCard;

        for (int i = 0; i != PlayerStatsInstance.Deck.Count; i++)
        {
            GameObject playerCard = Instantiate(CardPrefab, transform); //  where a card is instantiated from the list
            playerCard.GetComponent<CardStats>().CardAsset = PlayerStatsInstance.Deck[i]; // give the card it's stats
            playerCard.transform.SetParent(CardArea.transform, false); // set the parent to the grid
            
            CardList.Add(playerCard); // add all of the cards in a list to keep track
           
        }
    }

    

   
    void Update()
    {
        CardCounter.text = CustomDeck.Count.ToString();
    }

    
    public void CleanDeck()
    {
        CustomDeck.Clear();
    }

    // a function to export CustomDeck to a text file
    public void ExportDeck()
    {
        string path = Application.persistentDataPath + "/Decks/" + DeckNameInput.text + ".txt";
        if (Directory.Exists(Application.persistentDataPath + "/Decks"))
        {
            File.WriteAllLines(path, CustomDeck.Select(x => x.GetComponent<CardStats>().CardAsset.name).ToArray());
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Decks");
            File.WriteAllLines(path, CustomDeck.Select(x => x.GetComponent<CardStats>().CardAsset.name).ToArray());
        }
    }

    // a function to import a text file to CustomDeck
    public void ImportDeck()
    {
        string path = Application.persistentDataPath + "/Decks/" + ImportInput.text + ".txt";
        //  string path = "Assets/Resources/Decks/id test deck.txt";
        List<Card> cardList = System.IO.File.ReadAllLines(path).ToList().ConvertAll(item => (Card)Resources.Load("Cards/" + item));
        
        foreach (Card card in cardList) {
            GameObject playerCard;
            CustomDeck.Add(playerCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity));
            playerCard.GetComponent<CardStats>().CardAsset = card;
        }

        for (int i = 0; i != CustomDeck.Count; i++)
        {
            CustomDeck[i].transform.SetParent(ScrollviewParent.transform, false); // set the cards as children of the scroll view
            
        }
    }

    
    //a function that checks if cardlist has duplicate cards and stores how many duplicate cards there are
    public int CheckDuplicates(GameObject currentCard)
    {
        int duplicates = 0;

        string orignalCard = currentCard.GetComponent<CardStats>().CardNameText.text;
        
         for (int i = 0; i != CustomDeck.Count; i++)
         {
            string dupedCard = CustomDeck[i].GetComponent<CardStats>().CardNameText.text;

            if (dupedCard == orignalCard)
             {
                 duplicates++;
             }

           // Debug.Log(CustomDeck[i].GetComponent<CardStats>().CardNameText.ToString());
          //  Debug.Log(orignalCard.ToString());
         //   Debug.Log(dupedCard.ToString());
          }
      
        Debug.Log("Duplicates: " + duplicates);

        return duplicates;
           
    }


}
