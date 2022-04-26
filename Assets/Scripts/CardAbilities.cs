using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilities : MonoBehaviour
{
    public static void EffieAbility(GameManager gameManager, CardStats caster)
    {
        //Get ally cards
        List<GameObject> targets = (caster.BelongsToLocalPlayer) ? 
            gameManager.Player.GetComponent<PlayerStats>().FieldCards :
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards;

        foreach (GameObject target in targets)
        {
            //Check if damaged, and increment health if so
            if (target.GetComponent<CardStats>().Health < target.GetComponent<CardStats>().CardAsset.Health)
            {
                target.GetComponent<CardStats>().Health++;
            }
        }
    }

    public static void DonaldAbility(GameManager gameManager, CardStats caster)
    {
        int casterId = caster.ZoneID;

        //Get opponent cards
        List<GameObject> targets = (caster.BelongsToLocalPlayer) ?
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards :
            gameManager.Player.GetComponent<PlayerStats>().FieldCards;

        foreach (var target in targets)
        {
            if (casterId == target.GetComponent<CardStats>().ZoneID)
            {
                target.GetComponent<CardStats>().Health -= 2;
            } else if (casterId - 1 == target.GetComponent<CardStats>().ZoneID ||
                       casterId + 1 == target.GetComponent<CardStats>().ZoneID)
            {
                target.GetComponent<CardStats>().Health -= 1;
            }
        }
    }

    public static void MoragAbility(GameManager gameManager, CardStats caster)
    {
        GameObject target;
        
        if (caster.BelongsToLocalPlayer)
        {
            //find card in fieldcards with the zoneid of the caster - 1
            target = gameManager.Player.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }
        else
        {
            //find card in fieldcards with the zoneid of the caster + 1
            target = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }

        if (target != null)
        {
            //Increase target health and attack by 1
            target.GetComponent<CardStats>().Health++;
            target.GetComponent<CardStats>().Attack++;
        }
    }

    public static void GregorAbility(GameManager gameManager, CardStats caster)
    {
        GameObject target;

        if (caster.BelongsToLocalPlayer)
        {
            //find card in fieldcards with the zoneid of the caster - 1
            target = gameManager.Player.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }
        else
        {
            //find card in fieldcards with the zoneid of the caster + 1
            target = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }

        //reset target cardstats health, attack and mana cost back to their default values
        if (target != null)
        {
            target.GetComponent<CardStats>().Health = target.GetComponent<CardStats>().CardAsset.Health;
            target.GetComponent<CardStats>().Attack = target.GetComponent<CardStats>().CardAsset.Attack;
            target.GetComponent<CardStats>().ManaCost = target.GetComponent<CardStats>().CardAsset.ManaCost;
        }
    }

    public static void MatthewAbility(GameManager gameManager, CardStats caster)
    {
        int casterId = caster.ZoneID;

        //Get opponent cards
        List<GameObject> targets = (caster.BelongsToLocalPlayer) ?
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards :
            gameManager.Player.GetComponent<PlayerStats>().FieldCards;

        foreach (var target in targets)
        {
            //if the target has the same zoneid as the caster, decrease its attack by 2
            if (casterId == target.GetComponent<CardStats>().ZoneID)
            {
                target.GetComponent<CardStats>().Attack -= 2;
            }
        }
    }

    public static void LachlanAbility(GameManager gameManager, CardStats caster)
    {

    }

    public static void EachyAbility(GameManager gameManager, CardStats caster)
    {
        //get card from opponent with matching zoneid
        GameObject target = (caster.BelongsToLocalPlayer) ?
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID) :
            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID);

        //if target is not null, decrease its health by 1
        if (target != null)
        {
            target.GetComponent<CardStats>().Health--;
        }
    }

    public static void CeasgAbility(GameManager gameManager, CardStats caster)
    {
        List<GameObject> targets = new List<GameObject>();

        //get 3 ally field cards at random and add them to target list, not adding the same card twice
        for (int i = 0; i < 3; i++)
        {
            GameObject target;

            //set target to random card in ally fieldcards
            target = (caster.BelongsToLocalPlayer) ? 
                gameManager.Player.GetComponent<PlayerStats>().FieldCards[UnityEngine.Random.Range(0, gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count)] : 
                gameManager.Opponent.GetComponent<PlayerStats>().FieldCards[UnityEngine.Random.Range(0, gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count)];

            if (target != null && !targets.Contains(target))
            {
                targets.Add(target);
            }
            else
            {
                i--;
            }
        }

        //for each target, increase its health by 1
        foreach (GameObject target in targets)
        {
            target.GetComponent<CardStats>().Health++;
        }
    }

    public static void CuSithAbility(GameManager gameManager, CardStats caster)
    {
        //check all opponent cards and if HasAttackedOpponent is true, decrease that cards attack and health by 1
        foreach (GameObject target in gameManager.Opponent.GetComponent<PlayerStats>().FieldCards)
        {
            if (target.GetComponent<CardStats>().HasAttackedOpponent)
            {
                target.GetComponent<CardStats>().Attack--;
                target.GetComponent<CardStats>().Health--;
            }
        }
    }
}
