using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public delegate bool Rule();
    public delegate void Action();

    public GameManager GameManager;

    public float TimeSinceStartOfTurn;

    public float MinTimeBetweenActions = 1f;
    public float MaxTimeBetweenActions = 3f;

    public bool IsActive = false;

    //pair of rule and action
    public class RuleActionPair
    {
        //constructor that takes in a rule and an action
        public RuleActionPair(Rule newRule, Action newAction)
        {
            rule = newRule;
            action = newAction;
        }

        public Rule rule;
        public Action action;
    }

    //list of rule and action pairs
    public List<RuleActionPair> ruleActionPairs;

    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        ruleActionPairs = new List<RuleActionPair>();
        ruleActionPairs.Add(new RuleActionPair(CheckIfPlayableCard, GetHighestManaCard));
        ruleActionPairs.Add(new RuleActionPair(CheckIfAttackableCard, AttackWithAllCards));
        ruleActionPairs.Add(new RuleActionPair(AlwaysTrue, EndTurn));
    }

    // Update is called once per frame
    void Update()
    {
        //runs through the cards in handcards and enables cardback
        foreach (GameObject card in playerStats.HandCards)
        {
            //card.GetComponent<CardStats>().CardBack.SetActive(true);
            card.GetComponent<DragDrop>().enabled = false;
        }

        //runs through the cards in fieldcards and disables cardback
        foreach (GameObject card in playerStats.FieldCards)
        {
            card.GetComponent<CardStats>().CardBack.SetActive(false);
        }

        //if players turn, check list of rules and actions and execute the first one that returns true
        if ((playerStats.IsLocalPlayer && GameManager.PlayersTurn) ||
            (!playerStats.IsLocalPlayer && !GameManager.PlayersTurn))
        {
            if (!IsActive)
            {
                IsActive = true;
                StartCoroutine(RunRules());
            }
        }
    }

    public IEnumerator RunRules()
    {
        foreach (RuleActionPair pair in ruleActionPairs)
        {
            yield return new WaitForSeconds(Random.Range(MinTimeBetweenActions, MaxTimeBetweenActions));
            if (pair.rule())
            {
                //yield for random amount of time between minactiontime and maxactiontime
                pair.action();
                break;
            }
            
            yield return new WaitForSeconds(Random.Range(MinTimeBetweenActions, MaxTimeBetweenActions));
            IsActive = false;
        }
    }

    public void PlayCard()
    {
        //Call card play function on drag drop component
    }

    public void AttackWithCard(GameObject card)
    {
        //Targeting logic

        // find card in fieldcards of other player playerstats that matches the zoneid of card
        if (gameObject == GameManager.Player)
        {
            //find card in fieldcards of opponent that matches the zoneid of card
            foreach (GameObject opponentCard in GameManager.Opponent.GetComponent<PlayerStats>().FieldCards)
            {
                if (opponentCard.GetComponent<CardStats>().ZoneID == card.GetComponent<CardStats>().ZoneID)
                {
                    //call attack function on card
                    GameManager.CardAttackCard(card, opponentCard);
                    break;
                }
            }
        }
        else if
            (gameObject == GameManager.Opponent)
        {
            //find card in fieldcards of player that matches the zoneid of card
            foreach (GameObject playerCard in GameManager.Player.GetComponent<PlayerStats>().FieldCards)
            {
                if (playerCard.GetComponent<CardStats>().ZoneID == card.GetComponent<CardStats>().ZoneID)
                {
                    //call attack function on card
                    GameManager.CardAttackCard(card, playerCard);
                    break;
                }
            }
        }
    }

    public void InvokeCardAbility()
    {
        //Also targeting logic
    }

    public void EndTurn()
    {
        GameManager.EndTurn();
    }

    #region Rules
    //function that returns true
    public bool AlwaysTrue()
    {
        return true;
    }

    //function that checks handcards of AI and returns true if it has a card that can be played based on mana
    public bool CheckIfPlayableCard()
    {
        foreach (GameObject card in playerStats.HandCards)
        {
            if (card.GetComponent<CardStats>().ManaCost <= playerStats.Mana)
            {
                return true;
            }
        }
        return false;
    }

    //function that checks fieldcards and returns true if it has a card that can attack
    public bool CheckIfAttackableCard()
    {
        foreach (GameObject card in playerStats.FieldCards)
        {
            if (!card.GetComponent<CardStats>().FirstTurnPlayed && !playerStats.IsFirstTurn)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Actions
    //function that returns card in hand of highest mana cost
    public void GetHighestManaCard()
    {
        GameObject highestManaCard = null;
        int highestMana = 0;
        foreach (GameObject card in playerStats.HandCards)
        {
            if (card.GetComponent<CardStats>().ManaCost >= highestMana && card.GetComponent<CardStats>().ManaCost <= playerStats.Mana)
            {
                highestMana = card.GetComponent<CardStats>().ManaCost;
                highestManaCard = card;
            }
        }

        PlayCardInFirstAvailableZone(highestManaCard);

        
    }

    private void PlayCardInFirstAvailableZone(GameObject card)
    {
        //get card drag drop component
        DragDrop dragDrop = card.GetComponent<DragDrop>();
        
        for (int i = 0; i < 5; i++)
        {
            if (!GameManager.OpponentZones[i].GetComponent<DroppingZone>().IsBeingUsed)
            {
                card.GetComponent<CardStats>().ZoneID = i + 1;
                if (card.GetComponent<CardStats>().BelongsToLocalPlayer)
                {
                    dragDrop.PlayCardToZone(GameManager.PlayerZones[i]);
                }
                else 
                {
                    dragDrop.PlayCardToZone(GameManager.OpponentZones[i]);
                }
                
                break;
            }
        }
    }

    //function that attacks with all available cards
    public void AttackWithAllCards()
    {
        //get all cards that can attack
        List<GameObject> attackableCards = new List<GameObject>();
        foreach (GameObject card in playerStats.FieldCards)
        {
            if (!card.GetComponent<CardStats>().FirstTurnPlayed && !playerStats.IsFirstTurn)
            {
                attackableCards.Add(card);
            }
        }

        //attack with each card
        foreach (GameObject card in attackableCards)
        {
            AttackWithCard(card);
        }
    }

    #endregion

}
