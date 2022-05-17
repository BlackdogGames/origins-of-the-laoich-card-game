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

    public GameObject[] PlayerZones;
    public GameObject[] OpponentZones;

    // Used to randomise deck order
    System.Random _rng = new System.Random();


    void Start()
    {
        PlayersTurn = true;

        OppStatsInstance = Opponent.GetComponent<PlayerStats>();
        PlayerStatsInstance = Player.GetComponent<PlayerStats>();

        string deckName = DropdownScript.FileName; // get the selected file name fromm the dropdown menu
        string path = Application.persistentDataPath + "/Decks/" + deckName; // append the filepath
        //  PlayerStatsInstance.Deck = Resources.LoadAll("Cards").ToList().ConvertAll(item => (Card)item);
        PlayerStatsInstance.Deck = System.IO.File.ReadAllLines(path).ToList().ConvertAll(item => (Card)Resources.Load("Cards/" + item));
        OppStatsInstance.Deck = System.IO.File.ReadAllLines(Application.persistentDataPath + "/Decks/Default.txt").ToList().ConvertAll(item => (Card)Resources.Load("Cards/" + item));

        // Randomise player deck order
        PlayerStatsInstance.Deck = PlayerStatsInstance.Deck.OrderBy(card => _rng.Next()).ToList();
        OppStatsInstance.Deck = OppStatsInstance.Deck.OrderBy(card => _rng.Next()).ToList();

        //look through the deck and move the first card with a mana cost of 1 to the top of the deck
        for (int i = 0; i < PlayerStatsInstance.Deck.Count; i++)
        {
            if (PlayerStatsInstance.Deck[i].ManaCost == 1)
            {
                Card temp = PlayerStatsInstance.Deck[i];
                PlayerStatsInstance.Deck.RemoveAt(i);
                PlayerStatsInstance.Deck.Insert(0, temp);
                break;
            }
        }

        // Set player and opponent maxmana to 1
        PlayerStatsInstance.MaxMana = 1;
        OppStatsInstance.MaxMana = 1;

        // Set player and opponent current mana to 1
        PlayerStatsInstance.Mana = PlayerStatsInstance.MaxMana;
        OppStatsInstance.Mana = OppStatsInstance.MaxMana;

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

        foreach (var card in PlayerStatsInstance.FieldCards)
        {
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
        attackingCard.GetComponent<CardAnimator>().AnimateCardAttack(); // play the card animation

        if (attackingCard.GetComponent<CardStats>().CardAsset.AbilityTrigger == Card.CardAbilityTrigger.OnAttacking)
        {
            //call card ability
            attackingCard.GetComponent<CardStats>().CardAsset.Ability
                .Invoke(this, defendingCard.GetComponent<CardStats>());
        }

        if (defendingCard.GetComponent<CardStats>().CardAsset.AbilityTrigger == Card.CardAbilityTrigger.OnDefending)
        {
            //call card ability
            defendingCard.GetComponent<CardStats>().CardAsset.Ability
                .Invoke(this, defendingCard.GetComponent<CardStats>());
        }

        //check if input gameobjects are cards by checking if GetComponent<CardStats>() returns null

        // Subtracts the attacking cards attack damage from the defending cards health
        _attackingCard = attackingCard.GetComponent<CardStats>();
        _defendingCard = defendingCard.GetComponent<CardStats>();
        _defendingCard.Damage(_attackingCard.Attack);

        // Check for death of cards, delete from scene if dead
        if (_defendingCard.Health <= 0)
        {
            //DestroyCard(defendingCard);
        }
        else
        {
            defendingCard.GetComponent<CardAnimator>().AnimateCardHit(); // play the card animation}
        }
    }

    public void DestroyCard(GameObject defendingCard)
    {
        //remove card from field list
        if (defendingCard.GetComponent<CardStats>().BelongsToLocalPlayer)
        {
            PlayerStatsInstance.FieldCards.Remove(defendingCard);
            PlayerZones[defendingCard.GetComponent<CardStats>().ZoneID - 1].GetComponent<DroppingZone>()
                .IsBeingUsed = false;
        }
        else
        {
            OppStatsInstance.FieldCards.Remove(defendingCard);
            OpponentZones[defendingCard.GetComponent<CardStats>().ZoneID - 1].GetComponent<DroppingZone>()
                .IsBeingUsed = false;
        }

        if (defendingCard.GetComponent<CardStats>().CardAsset.AbilityTrigger == Card.CardAbilityTrigger.OnDestroyed)
        {
            //call card ability
            defendingCard.GetComponent<CardStats>().CardAsset.Ability.Invoke(this, defendingCard.GetComponent<CardStats>());
        }

        AudioManager.Instance.Play("SFX_Card_Destroyed");
        Destroy(defendingCard);
        Debug.Log("dead");
    }

    public void CardAttackPlayer(GameObject attackingCard, GameObject defendingPlayer)
    {
        attackingCard.GetComponent<CardAnimator>().AnimateCardAttack(); // play the card animation
        
        // Subracts the cards attack damage from the player health
        _attackingCard = attackingCard.GetComponent<CardStats>();
        PlayerStats defendingPlayerStats = defendingPlayer.GetComponent<PlayerStats>();
        defendingPlayerStats.Health -= _attackingCard.Attack;
        AudioManager.Instance.Play("SFX_Card_Attack");

        // If player dies, display defeat panel 
        if (defendingPlayerStats.Health <= 0)
        {
            if (defendingPlayerStats == PlayerStatsInstance)
            {
                DefeatPanel.SetActive(true);
                AudioManager.Instance.Play("SFX_Defeat"); // Can't test
            }

            else if (defendingPlayerStats == OppStatsInstance)
            {
                VictoryPanel.SetActive(true);
                AudioManager.Instance.Play("SFX_Victory"); // Can't test
            }

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
        }
        playerStats.Mana = playerStats.MaxMana;   // Set mana to the new max at the start of the new round
    }

    public void DrawCard(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats.Deck.Count > 0)
        {
            GameObject playerCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity); //  where a random card is instantiated from the list
            playerCard.GetComponent<CardStats>().CardAsset = playerStats.Deck[0];
            playerStats.Deck.RemoveAt(0);
            playerCard.transform.SetParent((playerStats.IsLocalPlayer) ? PlayerArea.transform : OpponentArea.transform, false); // when object is instantiated, set it as child of PlayerArea
            playerStats.HandCards.Add(playerCard);

            //set playercard firstturnplayed to false
            playerCard.GetComponent<CardStats>().FirstTurnPlayed = false;

            // if player stats is the local player, set the card belongs to local player to true
            if (playerStats.IsLocalPlayer)
            {
                playerCard.GetComponent<CardStats>().BelongsToLocalPlayer = true;
            }
            else
            {
                playerCard.GetComponent<CardStats>().BelongsToLocalPlayer = false;
            }
        }

        if (playerStats.Deck.Count == 0)
        {
            //if local player, display defeat panel, else display victory panel
            if (playerStats.IsLocalPlayer)
            {
                DefeatPanel.SetActive(true);
                AudioManager.Instance.Stop("GameMusic");
                AudioManager.Instance.Play("Victory_Defeat_Music");
                AudioManager.Instance.Play("SFX_Defeat");
            }
            else
            {
                VictoryPanel.SetActive(true);
                AudioManager.Instance.Stop("GameMusic");
                AudioManager.Instance.Play("Victory_Defeat_Music");
                AudioManager.Instance.Play("SFX_Victory");
            }
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
        GameObject.Find("End Turn Button").GetComponent<EndTurnButton>().OnClick();
    }
}

