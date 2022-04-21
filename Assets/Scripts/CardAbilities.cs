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
        GameObject target = DragDrop.CardList[1];

        //Increase target health and attack by 1
        target.GetComponent<CardStats>().Health++;
        target.GetComponent<CardStats>().Attack++;
    }
}
