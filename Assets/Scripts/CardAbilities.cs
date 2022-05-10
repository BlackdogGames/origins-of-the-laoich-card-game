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
        List<GameObject> targets = (caster.BelongsToLocalPlayer)
            ? gameManager.Player.GetComponent<PlayerStats>().FieldCards
            : gameManager.Opponent.GetComponent<PlayerStats>().FieldCards;

        foreach (GameObject target in targets)
        {
            //Check if damaged, and increment health if so
            if (target.GetComponent<CardStats>().Health < target.GetComponent<CardStats>().CardAsset.Health)
            {
                target.GetComponent<CardStats>().Health++;
            }
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Heal");
    }

    public static void DonaldAbility(GameManager gameManager, CardStats caster)
    {
        int casterId = caster.ZoneID;

        //Get opponent cards
        List<GameObject> targets = (caster.BelongsToLocalPlayer)
            ? gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
            : gameManager.Player.GetComponent<PlayerStats>().FieldCards;

        foreach (var target in targets)
        {
            if (casterId == target.GetComponent<CardStats>().ZoneID)
            {
                target.GetComponent<CardStats>().Damage(2);
            }
            else if (casterId - 1 == target.GetComponent<CardStats>().ZoneID ||
                     casterId + 1 == target.GetComponent<CardStats>().ZoneID)
            {
                target.GetComponent<CardStats>().Damage(1);
            }
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
    }

    public static void MoragAbility(GameManager gameManager, CardStats caster)
    {
        GameObject target;

        if (caster.BelongsToLocalPlayer)
        {
            //find card in fieldcards with the zoneid of the caster - 1
            target = gameManager.Player.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }
        else
        {
            //find card in fieldcards with the zoneid of the caster + 1
            target = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }

        if (target != null)
        {
            //Increase target health and attack by 1
            target.GetComponent<CardStats>().Health++;
            target.GetComponent<CardStats>().Attack++;
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
    }

    public static void GregorAbility(GameManager gameManager, CardStats caster)
    {
        GameObject target;

        if (caster.BelongsToLocalPlayer)
        {
            //find card in fieldcards with the zoneid of the caster - 1
            target = gameManager.Player.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }
        else
        {
            //find card in fieldcards with the zoneid of the caster + 1
            target = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }

        //reset target cardstats health, attack and mana cost back to their default values
        if (target != null)
        {
            target.GetComponent<CardStats>().Health = target.GetComponent<CardStats>().CardAsset.Health;
            target.GetComponent<CardStats>().Attack = target.GetComponent<CardStats>().CardAsset.Attack;
            target.GetComponent<CardStats>().ManaCost = target.GetComponent<CardStats>().CardAsset.ManaCost;
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
    }

    public static void MatthewAbility(GameManager gameManager, CardStats caster)
    {
        int casterId = caster.ZoneID;

        //Get opponent cards
        List<GameObject> targets = (caster.BelongsToLocalPlayer)
            ? gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
            : gameManager.Player.GetComponent<PlayerStats>().FieldCards;

        foreach (var target in targets)
        {
            //if the target has the same zoneid as the caster, decrease its attack by 2
            if (casterId == target.GetComponent<CardStats>().ZoneID)
            {
                target.GetComponent<CardStats>().Attack -= 2;
            }
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
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
        GameObject target = (caster.BelongsToLocalPlayer)
            ? gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID)
            : gameManager.Player.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID);

        //if target is not null, decrease its attack by 1
        if (target != null)
        {
            target.GetComponent<CardStats>().Attack--;
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
    }

    public static void CeasgAbility(GameManager gameManager, CardStats caster)
    {
        List<GameObject> targets = new List<GameObject>();

        //get count of cards in fieldcards of opponent
        int count = (caster.BelongsToLocalPlayer)
            ? gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count
            : gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count;

        //get 3 ally field cards at random and add them to target list, not adding the same card twice
        for (int i = 0; i < Mathf.Min(count, 3); i++)
        {
            GameObject target;

            //set target to random card in ally fieldcards
            target = (caster.BelongsToLocalPlayer)
                ? gameManager.Player.GetComponent<PlayerStats>().FieldCards[
                    UnityEngine.Random.Range(0, gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count)]
                : gameManager.Opponent.GetComponent<PlayerStats>().FieldCards[
                    UnityEngine.Random.Range(0, gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count)];

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

        AudioManager.Instance.Play("SFX_Card_Ability_Heal");
    }

    public static void CuSithAbility(GameManager gameManager, CardStats caster)
    {
        //check all opponent cards and if HasAttackedOpponent is true, decrease that cards attack and health by 1
        foreach (GameObject target in gameManager.Opponent.GetComponent<PlayerStats>().FieldCards)
        {
            if (target.GetComponent<CardStats>().HasAttackedOpponent)
            {
                target.GetComponent<CardStats>().Attack--;
                target.GetComponent<CardStats>().Damage(1);
            }
        }

        AudioManager.Instance.Play("SFX_Card_Ability_Howl");
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
        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
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
        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
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
        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
    }

    public static void PhantomPiperAbility(GameManager gameManager, CardStats caster)
    {
        if (caster.BelongsToLocalPlayer)
        {
            //pick random card in opponent zones and spawn it on players side
            if (gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count != 5)
            {
                GameObject stolenCard;

                int count = UnityEngine.Random.Range(0,
                    gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Count - 1);
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

                int count = UnityEngine.Random.Range(0,
                    gameManager.Player.GetComponent<PlayerStats>().FieldCards.Count - 1);
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
        AudioManager.Instance.Play("SFX_Card_Ability_Generic");
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

        }
        // Destroy spell card
        DestroySpell(gameManager, caster);
        AudioManager.Instance.Play("SFX_Card_Ability_Heal");
    }

    public static void WaterBubbleAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get the player that is not the local player
        GameObject otherPlayer = isLocalPlayer ? gameManager.Opponent : gameManager.Player;

        // Get field cards of the other player
        List<GameObject> otherPlayerFieldCards = otherPlayer.GetComponent<PlayerStats>().FieldCards;

        // Find fieldcard that's matches zoneid of caster
        GameObject fieldCard = otherPlayerFieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID);

        // If fieldcard is found, set firstturnplayed to true
        if (fieldCard != null)
        {
            fieldCard.GetComponent<CardStats>().FirstTurnPlayed = true;
        }

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void TidalWaveAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get the player that is not the local player
        GameObject otherPlayer = isLocalPlayer ? gameManager.Opponent : gameManager.Player;

        // Get field cards of the other player
        List<GameObject> otherPlayerFieldCards = otherPlayer.GetComponent<PlayerStats>().FieldCards;

        // Damage all field cards of the other player by 2
        foreach (GameObject card in otherPlayerFieldCards)
        {
            card.GetComponent<CardStats>().Damage(2);
        }

        // Get field cards of the local player
        List<GameObject> localPlayerFieldCards = gameManager.Player.GetComponent<PlayerStats>().FieldCards;

        // Heal all field cards of the local player by 2
        foreach (GameObject card in localPlayerFieldCards)
        {
            card.GetComponent<CardStats>().Health += 2;
        }

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void TakeAimAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get the player that is not the local player
        GameObject otherPlayer = isLocalPlayer ? gameManager.Opponent : gameManager.Player;

        // Deal 2 damage to other player
        otherPlayer.GetComponent<PlayerStats>().Health -= 2;

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void TargeAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get the player that is not the local player
        GameObject otherPlayer = isLocalPlayer ? gameManager.Opponent : gameManager.Player;

        // Get field cards of the local player
        List<GameObject> localPlayerFieldCards = gameManager.Player.GetComponent<PlayerStats>().FieldCards;

        // Heal all field cards of the local player by 2
        foreach (GameObject card in localPlayerFieldCards)
        {
            card.GetComponent<CardStats>().Health += 2;
        }

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void OtterAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get both field card lists
        List<GameObject> localPlayerFieldCards = gameManager.Player.GetComponent<PlayerStats>().FieldCards;
        List<GameObject> otherPlayerFieldCards = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards;

        // Temporary variable
        int healthIncrease = 1;

        // If any card in the field card lists is named "The Boobrie", return true
        foreach (GameObject card in localPlayerFieldCards)
        {
            if (card.GetComponent<CardStats>().CardName == "The Boobrie")
            {
                healthIncrease = 2;
                break;
            }
        }
        
        foreach (GameObject card in otherPlayerFieldCards)
        {
            if (card.GetComponent<CardStats>().CardName == "The Boobrie")
            {
                healthIncrease = 2;
                break;
            }
        }

        // Get card to the right of the caster
        GameObject cardToTheRight;
        if (isLocalPlayer)
        {
            cardToTheRight = localPlayerFieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }
        else
        {
            cardToTheRight = otherPlayerFieldCards.Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }

        // Heal card to the right of the caster by healthIncrease
        if (cardToTheRight != null)
        {
            cardToTheRight.GetComponent<CardStats>().Health += healthIncrease;
        }

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void PerfectDateAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get card to the right of the caster
        GameObject cardToTheRight;
        if (isLocalPlayer)
        {
            cardToTheRight = gameManager.Player.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }
        else
        {
            cardToTheRight = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }

        // If card to the right is found, set invincible to true
        if (cardToTheRight != null)
        {
            cardToTheRight.GetComponent<CardStats>().Invincible = true;
        }

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void ConservationAbility(GameManager gameManager, CardStats caster)
    {
        // Get which players turn it is
        bool isLocalPlayer = caster.BelongsToLocalPlayer;

        // Get card to the left of the caster
        GameObject cardToTheLeft;
        if (isLocalPlayer)
        {
            cardToTheLeft = gameManager.Player.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID - 1);
        }
        else
        {
            cardToTheLeft = gameManager.Opponent.GetComponent<PlayerStats>().FieldCards
                .Find(x => x.GetComponent<CardStats>().ZoneID == caster.ZoneID + 1);
        }

        // If card to the left is found, set invincible to true
        if (cardToTheLeft != null)
        {
            cardToTheLeft.GetComponent<CardStats>().Invincible = true;
        }

        // Destroy spell card
        DestroySpell(gameManager, caster);
    }

    public static void DestroySpell(GameManager gameManager, CardStats card)
    {
        // If card belongs to local player, remove it from the field card list
        if (card.BelongsToLocalPlayer)
        {
            gameManager.Player.GetComponent<PlayerStats>().FieldCards.Remove(card.gameObject);
            gameManager.PlayerZones[card.ZoneID - 1].GetComponent<DroppingZone>().IsBeingUsed = false;
            Destroy(card.gameObject);
        }
        else
        {
            gameManager.Opponent.GetComponent<PlayerStats>().FieldCards.Remove(card.gameObject);
            gameManager.OpponentZones[card.ZoneID - 1].GetComponent<DroppingZone>().IsBeingUsed = false;
            Destroy(card.gameObject);
        }
        
    }
}