using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardStats : MonoBehaviour
{
    public Card CardAsset;
    public int ManaCost;
    public int Attack;
    public int Health;
    public int ZoneID = 0; // attack zone ID
    public bool IsDuplicate; // is a duped card - used in deckbuilding
    public bool isMonster; // true is a monster, false is a spell

    public string CardName;

    public bool BelongsToLocalPlayer;
    public bool FirstTurnPlayed = false;
    public bool HasAttackedOpponent = false;

    public bool Invincible = false;

    public GameObject[] MonsterObjects;
    public GameObject[] SpellObjects;

    // Assigned through the inspector
    public TMP_Text CardNameText;
    public TMP_Text ManaCostText;
    public TMP_Text AttackText;
    public TMP_Text HealthText;
    public TMP_Text DescriptionText;

    public GameObject CardBack;

    public Image CardImage;

    void Start()
    {
        ManaCost = CardAsset.ManaCost;
        Attack = CardAsset.Attack;
        Health = CardAsset.Health;
        CardImage.sprite = CardAsset.CardImage;
        CardNameText.text = CardAsset.CardName;
        DescriptionText.text = CardAsset.Description;
        isMonster = CardAsset.IsMonster;

        // name
        CardName = CardAsset.CardName; // name of card
        // image
        ManaCostText.text = ManaCost.ToString();
        AttackText.text = Attack.ToString();
        HealthText.text = Health.ToString();

        if (CardAsset.IsMonster)
        {
            // Enable monster objects
            for (int i = 0; i < MonsterObjects.Length; i++)
            {
                MonsterObjects[i].SetActive(true);
            }

            // Disable spell objects
            for (int i = 0; i < SpellObjects.Length; i++)
            {
                SpellObjects[i].SetActive(false);
            }
        }
        else
        {
            // Enable spell objects
            for (int i = 0; i < SpellObjects.Length; i++)
            {
                SpellObjects[i].SetActive(true);
            }

            // Disable monster objects
            for (int i = 0; i < MonsterObjects.Length; i++)
            {
                MonsterObjects[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        ManaCostText.text = ManaCost.ToString();
        AttackText.text = Attack.ToString();
        HealthText.text = Health.ToString();

        if (!isMonster && FirstTurnPlayed && ZoneID != 0)
        {
            Damage(999);
        }
    }

    public void Damage(int amount)
    {
        if (!Invincible && amount >= 0)
        {
            Health -= amount;
            // Calls AudioManager to PLay a requested Sound.
            AudioManager.Instance.Play("SFX_Card_Attack");
        }
        else
        {
            // Calls AudioManager to PLay a requested Sound.
            AudioManager.Instance.Play("SFX_Card_Block");
        }


        //if health is lower than or equal to 0, remove card from players fieldlist, set zone used to false, and destroy card
        if (Health <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DestroyCard(gameObject);
        }
    }
}
