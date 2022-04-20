using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject PlayerZone1;
    public GameObject PlayerZone2;
    public GameObject PlayerZone3;
    public GameObject PlayerZone4;
    public GameObject PlayerZone5;

    public GameObject OpponentZone1;
    public GameObject OpponentZone2;
    public GameObject OpponentZone3;
    public GameObject OpponentZone4;
    public GameObject OpponentZone5;

    public delegate bool Rule();
    public delegate void Action();

    public GameManager GameManager;

    public float TimeSinceStartOfTurn;

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
        ruleActionPairs.Add(new RuleActionPair(AlwaysTrue, EndTurn));
    }

    // Update is called once per frame
    void Update()
    {
        //runs through the cards in handcards and enables cardback
        foreach (GameObject card in playerStats.HandCards)
        {
            card.GetComponent<CardStats>().CardBack.SetActive(true);
            card.GetComponent<DragDrop>().enabled = false;
        }

        //runs through the cards in fieldcards and disables cardback
        foreach (GameObject card in playerStats.FieldCards)
        {
            card.GetComponent<CardStats>().CardBack.SetActive(false);
        }

        TimeSinceStartOfTurn += Time.deltaTime;

        //if players turn, check list of rules and actions and execute the first one that returns true
        if ((playerStats.IsLocalPlayer && GameManager.PlayersTurn) || (!playerStats.IsLocalPlayer && !GameManager.PlayersTurn))
        {
            //wait 2 seconds before checking rules
            if (TimeSinceStartOfTurn > 2)
            {
                foreach (RuleActionPair pair in ruleActionPairs)
                {
                    if (pair.rule())
                    {
                        pair.action();
                        break;
                    }
                }
            }
        }
        else
        {
            TimeSinceStartOfTurn = 0;
        }
    }

    public void PlayCard()
    {
        //Call card play function on drag drop component
    }

    public void AttackWithCard()
    {
        //Targeting logic
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
    #endregion

    #region Actions
    //function that returns card in hand of highest mana cost
    public void GetHighestManaCard()
    {
        GameObject highestManaCard = null;
        int highestMana = 0;
        foreach (GameObject card in playerStats.HandCards)
        {
            if (card.GetComponent<CardStats>().ManaCost > highestMana && card.GetComponent<CardStats>().ManaCost <= playerStats.Mana)
            {
                highestMana = card.GetComponent<CardStats>().ManaCost;
                highestManaCard = card;
            }
        }

        //get card drag drop component
        DragDrop dragDrop = highestManaCard.GetComponent<DragDrop>();
        if (!OpponentZone1.GetComponent<DroppingZone>().IsBeingUsed)
        {
            dragDrop.PlayCardToZone(OpponentZone1);
            highestManaCard.GetComponent<CardStats>().ZoneID = 1;
        } 
        else if (!OpponentZone2.GetComponent<DroppingZone>().IsBeingUsed)
        {
            dragDrop.PlayCardToZone(OpponentZone2);
            highestManaCard.GetComponent<CardStats>().ZoneID = 2;
        } 
        else if (!OpponentZone3.GetComponent<DroppingZone>().IsBeingUsed)
        {
            dragDrop.PlayCardToZone(OpponentZone3);
            highestManaCard.GetComponent<CardStats>().ZoneID = 3;
        }
        else if (!OpponentZone4.GetComponent<DroppingZone>().IsBeingUsed)
        {
            dragDrop.PlayCardToZone(OpponentZone4);
            highestManaCard.GetComponent<CardStats>().ZoneID = 4;
        }
        else if (!OpponentZone5.GetComponent<DroppingZone>().IsBeingUsed)
        {
            dragDrop.PlayCardToZone(OpponentZone5);
            highestManaCard.GetComponent<CardStats>().ZoneID = 5;
        }


    }
    #endregion

}
