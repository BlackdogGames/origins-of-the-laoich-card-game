using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public delegate bool Rule();
    public delegate void Action();

    public GameManager GameManager;

    //pair of rule and action
    public class RuleActionPair
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        //if players turn, check list of rules and actions and execute the first one that returns true
        if ((playerStats.IsLocalPlayer && GameManager.PlayersTurn) || (!playerStats.IsLocalPlayer && !GameManager.PlayersTurn))
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
    public GameObject GetHighestManaCard()
    {
        GameObject highestManaCard = null;
        int highestMana = 0;
        foreach (GameObject card in playerStats.HandCards)
        {
            if (card.GetComponent<CardStats>().ManaCost > highestMana)
            {
                highestMana = card.GetComponent<CardStats>().ManaCost;
                highestManaCard = card;
            }
        }
        
        //TODO: Replace with playing this card

        return highestManaCard;
    }
    #endregion

}
