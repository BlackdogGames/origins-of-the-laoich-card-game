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

    
}
