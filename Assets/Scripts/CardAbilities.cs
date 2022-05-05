using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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
        GameObject zoneSummon = null;
        GameObject newCard = null;
        //check if caster belongs to player
        if (caster.GetComponent<CardStats>().BelongsToLocalPlayer)
        {
            zoneSummon = gameManager.PlayerZones[caster.GetComponent<CardStats>().ZoneID - 1];
            int zoneId = caster.GetComponent<CardStats>().ZoneID;
            newCard = Instantiate(gameManager.CardPrefab);

            newCard.GetComponent<CardStats>().CardAsset = (Card)Resources.Load("Cards/" + "Changeling");
            newCard.transform.SetParent(gameManager.PlayerArea.transform, false);
            newCard.GetComponent<CardStats>().ZoneID = zoneId;

            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Add(newCard);
        }
        else
        {
            zoneSummon = gameManager.OpponentZones[caster.GetComponent<CardStats>().ZoneID - 1];
            int zoneId = caster.GetComponent<CardStats>().ZoneID;
            newCard = Instantiate(gameManager.CardPrefab);

            newCard.GetComponent<CardStats>().CardAsset = (Card)Resources.Load("Cards/" + "Changeling");
            newCard.transform.SetParent(gameManager.OpponentArea.transform, false);
            newCard.GetComponent<CardStats>().ZoneID = zoneId;

            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Add(newCard);
        }

        newCard.GetComponent<DragDrop>().PlayCardToZone(zoneSummon, false);
    }

    public static void EachyAbility(GameManager gameManager, CardStats caster)
    {
        //get card from opponent with matching zoneid
        GameObject target = (caster.BelongsToLocalPlayer) ?
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID) :
            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID);

        //if target is not null, decrease its attack by 1
        if (target != null)
        {
            target.GetComponent<CardStats>().Attack--;
        }
    }

    public static void CeasgAbility(GameManager gameManager, CardStats caster)
    {
        List<GameObject> targets = new List<GameObject>();

        //get count of cards in fieldcards of opponent
        int count = (caster.BelongsToLocalPlayer) ?
            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count :
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count;

        //get 3 ally field cards at random and add them to target list, not adding the same card twice
        for (int i = 0; i < Mathf.Min(count, 3); i++)
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

    public static void ArvilAbility(GameManager gameManager, CardStats caster)
    {
        if (caster.BelongsToLocalPlayer)
        {
            foreach (GameObject card in gameManager.Player.GetComponent<PlayerStats>().HandCards)
            {
                if (card.GetComponent<CardStats>().CardAsset.CardName == "Carwen")
                {
                    foreach (GameObject zone in gameManager.PlayerZones)
                    {
                        if (!zone.GetComponent<DroppingZone>().IsBeingUsed)
                        {
                            card.GetComponent<DragDrop>().PlayCardToZone(zone, false);
                            card.GetComponent<CardStats>().BelongsToLocalPlayer = true;
                            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Add(card);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject card in gameManager.Opponent.GetComponent<PlayerStats>().HandCards)
            {
                if (card.GetComponent<CardStats>().CardAsset.CardName == "Carwen")
                {
                    foreach (GameObject zone in gameManager.OpponentZones)
                    {
                        if (!zone.GetComponent<DroppingZone>().IsBeingUsed)
                        {
                            card.GetComponent<DragDrop>().PlayCardToZone(zone, false);
                            card.GetComponent<CardStats>().BelongsToLocalPlayer = false;
                            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Add(card);
                            break;
                        }
                    }
                }
            }
        }
    }
    public static void CarwenAbility(GameManager gameManager, CardStats caster)
    {
        if (caster.BelongsToLocalPlayer)
        {
            foreach (GameObject card in gameManager.Player.GetComponent<PlayerStats>().HandCards)
            {
                if (card.GetComponent<CardStats>().CardAsset.CardName == "Arvil")
                {
                    foreach (GameObject zone in gameManager.PlayerZones)
                    {
                        if (!zone.GetComponent<DroppingZone>().IsBeingUsed)
                        {
                            card.GetComponent<DragDrop>().PlayCardToZone(zone, false);
                            card.GetComponent<CardStats>().BelongsToLocalPlayer = true;
                            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Add(card);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject card in gameManager.Opponent.GetComponent<PlayerStats>().HandCards)
            {
                if (card.GetComponent<CardStats>().CardAsset.CardName == "Arvil")
                {
                    foreach (GameObject zone in gameManager.OpponentZones)
                    {
                        if (!zone.GetComponent<DroppingZone>().IsBeingUsed)
                        {
                            card.GetComponent<DragDrop>().PlayCardToZone(zone, false);
                            card.GetComponent<CardStats>().BelongsToLocalPlayer = false;
                            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Add(card);
                            break;
                        }
                    }
                }
            }
        }
    }
    public static void BlueMenAbility(GameManager gameManager, CardStats caster)
    {
        int i = 1;
        int numOfBlueMenSummoned = 0;
        int numOfBlueMenToSummon = 2;

        if (!caster.BelongsToLocalPlayer)
        {
            foreach (GameObject zone in gameManager.OpponentZones)
            {
                if (!zone.GetComponent<DroppingZone>().IsBeingUsed)
                {
                    GameObject newCard;
                    newCard = Instantiate(gameManager.CardPrefab);

                    newCard.GetComponent<CardStats>().CardAsset = (Card)Resources.Load("Cards/" + "Blue Man");
                    newCard.transform.SetParent(gameManager.OpponentArea.transform, false);
                    newCard.GetComponent<CardStats>().ZoneID = i;

                    //gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Add(newCard);

                    newCard.GetComponent<CardStats>().BelongsToLocalPlayer = false;
                    newCard.GetComponent<DragDrop>().PlayCardToZone(zone, false);

                    numOfBlueMenSummoned++;
                    if (numOfBlueMenSummoned == numOfBlueMenToSummon)
                    {
                        break;
                    }
                }

                i++;
            }
        }
        else
        {
            foreach (GameObject zone in gameManager.PlayerZones)
            {
                if (!zone.GetComponent<DroppingZone>().IsBeingUsed)
                {
                    GameObject newCard;
                    newCard = Instantiate(gameManager.CardPrefab);

                    newCard.GetComponent<CardStats>().CardAsset = (Card)Resources.Load("Cards/" + "Blue Man");
                    newCard.transform.SetParent(gameManager.PlayerArea.transform, false);
                    newCard.GetComponent<CardStats>().ZoneID = i;

                    //gameManager.Player.GetComponent<PlayerStats>().FieldCards.Add(newCard);

                    newCard.GetComponent<CardStats>().BelongsToLocalPlayer = true;
                    newCard.GetComponent<DragDrop>().PlayCardToZone(zone, false);

                    numOfBlueMenSummoned++;
                    if (numOfBlueMenSummoned == numOfBlueMenToSummon)
                    {
                        break;
                    }
                }

                i++;
            }
        }
    }

    public static void PhantomPiperAbility(GameManager gameManager, CardStats caster)
    {
        if (caster.BelongsToLocalPlayer)
        {
            //pick random card in opponent zones and spawn it on players side
            if (gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count != 5)
            {
                GameObject stolenCard;

                int count = UnityEngine.Random.Range(0, gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count - 1);
                stolenCard = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards[count];

                //gameManager.Player.GetComponent<PlayerStats>().FieldCards.Add(
                //    stolenCard
                //);

                gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Remove(
                    stolenCard
                );

                gameManager.OpponentZones[stolenCard.GetComponent<CardStats>().ZoneID - 1].GetComponent<DroppingZone>()
                    .IsBeingUsed = false;

                stolenCard.GetComponent<CardStats>().BelongsToLocalPlayer = true;

                for (int i = 0; i < 5; i++)
                {
                    if (!gameManager.PlayerZones[i].GetComponent<DroppingZone>().IsBeingUsed)
                    {
                        stolenCard.GetComponent<CardStats>().ZoneID = i + 1;
                        stolenCard.GetComponent<DragDrop>().PlayCardToZone(gameManager.PlayerZones[i], false);
                        break;
                    }
                }
            }
        }
        else
        {
            //pick random card in opponent zones and spawn it on players side
            if (gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count != 5)
            {
                GameObject stolenCard;

                int count = UnityEngine.Random.Range(0, gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count - 1);
                stolenCard = gameManager.Player.GetComponent<PlayerStats>().FieldCards[count];

                //gameManager.Player.GetComponent<PlayerStats>().FieldCards.Add(
                //    stolenCard
                //);

                gameManager.Player.GetComponent<PlayerStats>().FieldCards.Remove(
                    stolenCard
                );

                gameManager.PlayerZones[stolenCard.GetComponent<CardStats>().ZoneID - 1].GetComponent<DroppingZone>()
                    .IsBeingUsed = false;

                stolenCard.GetComponent<CardStats>().BelongsToLocalPlayer = false;

                for (int i = 0; i < 5; i++)
                {
                    if (!gameManager.OpponentZones[i].GetComponent<DroppingZone>().IsBeingUsed)
                    {
                        stolenCard.GetComponent<CardStats>().ZoneID = i + 1;
                        stolenCard.GetComponent<DragDrop>().PlayCardToZone(gameManager.OpponentZones[i], false);
                        break;
                    }
                }
            }
        }
    }

    public static void WaterHealingAbility(GameManager gameManager, CardStats caster)
    {
        if (caster.BelongsToLocalPlayer)
        {
            int targetId = caster.ZoneID + 1;

            foreach (GameObject card in gameManager.Player.GetComponent<PlayerStats>().FieldCards)
            {
                if (card.GetComponent<CardStats>().ZoneID == targetId)
                {
                    card.GetComponent<CardStats>().Health++;
                }
            }

            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Remove(caster.gameObject);
            gameManager.PlayerZones[caster.ZoneID - 1].GetComponent<DroppingZone>().IsBeingUsed = false;
            Destroy(caster.gameObject);
        }
        else
        {
            int targetId = caster.ZoneID - 1;

            foreach (GameObject card in gameManager.Opponent.GetComponent<PlayerStats>().FieldCards)
            {
                if (card.GetComponent<CardStats>().ZoneID == targetId)
                {
                    card.GetComponent<CardStats>().Health++;
                }
            }

            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Remove(caster.gameObject);
            gameManager.OpponentZones[caster.ZoneID - 1].GetComponent<DroppingZone>().IsBeingUsed = false;
            Destroy(caster.gameObject);
        }
    }
}
