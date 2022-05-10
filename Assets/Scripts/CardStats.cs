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
    public bool FirstTurnPlayed = true;
    public bool HasAttackedOpponent = false;

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
    }

    private void Update()
    {
        ManaCostText.text = ManaCost.ToString();
        AttackText.text = Attack.ToString();
        HealthText.text = Health.ToString();
    }
    
}
