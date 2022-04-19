using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public bool PlayersTurn; // True if players turn, false if opponents turn

    public TMP_Text TurnText;

    public GameObject Player;
    public GameObject Opponent;

    public GameObject MousePointer;

    public PlayerStats PlayerStatsInstance;
    public PlayerStats OppStatsInstance;

    CardStats _attackingCard;
    CardStats _defendingCard;

    public GameObject CardPrefab;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    public GameObject VictoryPanel, DefeatPanel;
    
    // Used to randomise deck order
    System.Random _rng = new System.Random();

    void Start()
    {
        PlayersTurn = true;

        OppStatsInstance = Opponent.GetComponent<PlayerStats>();
        PlayerStatsInstance = Player.GetComponent<PlayerStats>();

        PlayerStatsInstance.Deck = Resources.LoadAll("Cards").ToList().ConvertAll(item => (Card)item);
        OppStatsInstance.Deck = Resources.LoadAll("Cards").ToList().ConvertAll(item => (Card)item);

        // Randomise player deck order
        PlayerStatsInstance.Deck = PlayerStatsInstance.Deck.OrderBy(card => _rng.Next()).ToList();
        OppStatsInstance.Deck = OppStatsInstance.Deck.OrderBy(card => _rng.Next()).ToList();

        for (int i = 0; i < 5; i++)
        {
            DrawCard(Player);
            DrawCard(Opponent);
        }
    }

    public void ClearCardSelection()
    {
        foreach (var card in OppStatsInstance.HandCards)   // For each card in the players deck
        {
            if (!card)
            {
                OppStatsInstance.HandCards.Remove(card);
            }
            
            card.SendMessage("ClearSelectionList");
            
        }
        foreach (var card in PlayerStatsInstance.HandCards)
        {
            if (!card)
            {
                PlayerStatsInstance.HandCards.Remove(card);
            }

            card.SendMessage("ClearSelectionList");
        }
    }

    public void TurnLogic()
    {
       // Debug.Log(PlayersTurn);
        if (PlayersTurn)
        {
            //players turn
            //only access player cards
            foreach (var card in OppStatsInstance.HandCards)   // For each card in the players deck
            {
                if (!card)
                {
                    OppStatsInstance.HandCards.Remove(card);
                }

                card.GetComponent<DragDrop>().enabled = false;  // Disable Opponents Cards
            }
            foreach (var card in PlayerStatsInstance.HandCards)
            {
                if (!card)
                {
                    PlayerStatsInstance.HandCards.Remove(card);
                }

                card.GetComponent<DragDrop>().enabled = true;   // Enable Player Cards
            }
        }
        else
        {
            //opponents turn
            //only access opponent cards
            foreach (var card in OppStatsInstance.HandCards)   // For each card in the opponents deck
            {
                if (!card)
                {
                    OppStatsInstance.HandCards.Remove(card);
                }

                card.GetComponent<DragDrop>().enabled = true;   // Enable Opponents Cards
            }  
            foreach (var card in PlayerStatsInstance.HandCards)
            {
                //print("Disabling dragging");
                if (!card)
                {
                    PlayerStatsInstance.HandCards.Remove(card);
                }

                card.GetComponent<DragDrop>().enabled = false;  // Disable Player Cards
            }
        }
    }


    public bool IsCardPlayable(GameObject attackingCard, PlayerStats cardPlayer)
    {
        _attackingCard = attackingCard.GetComponent<CardStats>();
        

        if (cardPlayer.Mana < _attackingCard.ManaCost)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void CardAttackCard(GameObject attackingCard, GameObject defendingCard)
    {
        //check if input gameobjects are cards by checking if GetComponent<CardStats>() returns null

        // Subtracts the attacking cards attack damage from the defending cards health
        _attackingCard = attackingCard.GetComponent<CardStats>();
        _defendingCard = defendingCard.GetComponent<CardStats>();
        _defendingCard.Health -= _attackingCard.Attack;
        // Calls AudioManager to PLay a requested Sound.
        AudioManager.Instance.Play("SFX_Card_Attack");

        // Check for death of cards, delete from scene if dead
        if (_defendingCard.Health <= 0)
        {
            Destroy(defendingCard);
            Debug.Log("dead");
        }
    }

    public void CardAttackPlayer(GameObject attackingCard, GameObject defendingPlayer)
    {
        // Subracts the cards attack damage from the player health
        _attackingCard = attackingCard.GetComponent<CardStats>();
        PlayerStats defendingPlayerStats = defendingPlayer.GetComponent<PlayerStats>();
        defendingPlayerStats.Health -= _attackingCard.Attack;

        // If player dies, display defeat panel 
        if (defendingPlayerStats.Health <= 0)
        {
            if (defendingPlayerStats == PlayerStatsInstance)
            {
                DefeatPanel.SetActive(true);
                AudioManager.Instance.Play("SFX_Defeat"); // Can't test
            }

            // TODO: VictoryPanel.SetActive(true) for the winning player

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ManaDecrease(GameObject placedCard, PlayerStats cardPlayer)
    {
        // Subtracts the played cards mana cost from the player
        CardStats placedCardStats = placedCard.GetComponent<CardStats>();
        cardPlayer.Mana -= placedCardStats.ManaCost;
    }

    public void ManaIncrease(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats.MaxMana < 9)
        {
            playerStats.MaxMana += 1;    // Increment max mana by 1 every round.
            playerStats.Mana = playerStats.MaxMana;   // Set mana to the new max at the start of the new round
        }
       
    }

    public void DrawCard(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats.Deck.Count > 0 && playerStats.IsLocalPlayer)
        {
            GameObject playerCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity); //  where a random card is instantiated from the list
            playerCard.GetComponent<CardStats>().CardAsset = PlayerStatsInstance.Deck[0];
            playerStats.Deck.RemoveAt(0);
            playerCard.transform.SetParent(PlayerArea.transform, false); // when object is instantiated, set it as child of PlayerArea
            playerStats.HandCards.Add(playerCard);

            playerCard.GetComponent<CardStats>().BelongsToLocalPlayer = true;
        } else if (playerStats.Deck.Count > 0 && !playerStats.IsLocalPlayer)
        {
            GameObject enemyCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.GetComponent<CardStats>().CardAsset = PlayerStatsInstance.Deck[0];
            playerStats.Deck.RemoveAt(0);
            enemyCard.transform.SetParent(OpponentArea.transform, false); // child of opponent area
            playerStats.HandCards.Add(enemyCard);

            enemyCard.GetComponent<CardStats>().BelongsToLocalPlayer = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        TurnLogic();

        TurnText.text = PlayersTurn ? "Players turn" : "Opponents Turn";
    }

    //function that gets the end turn button and calls the onclick function
    public void EndTurn()
    {
        GameObject.Find("EndTurnButton").GetComponent<Button>().onClick.Invoke();
    }
}

//class that implements monte carlo tree search
//public class MCTS
//{
//    private int maxDepth = 5;
//    private int maxIterations = 100;
//    private int maxSimulations = 10;
//
//    public int MaxDepth
//    {
//        get { return maxDepth; }
//        set { maxDepth = value; }
//    }
//
//    public int MaxIterations
//    {
//        get { return maxIterations; }
//        set { maxIterations = value; }
//    }
//
//    public int MaxSimulations
//    {
//        get { return maxSimulations; }
//        set { maxSimulations = value; }
//    }
//
//    public int Evaluate(GameState state)
//    {
//        //return state.Evaluate();
//        return 0;
//    }
//
//    public GameState Select(GameState state)
//    {
//        //return state.Select();
//        return null;
//    }
//
//    public GameState Simulate(GameState state)
//    {
//        //return state.Simulate();
//        return null;
//    }
//
//    public GameState Search(GameState state)
//    {
//        int iterations = 0;
//        int simulations = 0;
//
//        while (iterations < maxIterations && simulations < maxSimulations)
//        {
//            GameState selected = Select(state);
//            GameState simulated = Simulate(selected);
//            int score = Evaluate(simulated);
//            state.Update(score);
//            simulations++;
//        }
//
//        return state;
//    }
//}