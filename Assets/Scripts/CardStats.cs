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
    public int ZoneID; // attack zone ID
    public bool IsDuplicate; // is a duped card - used in deckbuilding

    public bool BelongsToLocalPlayer;
    public bool FirstTurnPlayed = true;

    // Assigned through the inspector
    public TMP_Text CardNameText;
    public TMP_Text ManaCostText;
    public TMP_Text AttackText;
    public TMP_Text HealthText;

    public Image CardImage;

    void Start()
    {
        ManaCost = CardAsset.ManaCost;
        Attack = CardAsset.Attack;
        Health = CardAsset.Health;
        CardImage.sprite = CardAsset.CardImage;
        CardNameText.text = CardAsset.CardName;

        // name
        // image
        ManaCostText.text = ManaCost.ToString();
        AttackText.text = Attack.ToString();
        HealthText.text = Health.ToString();

        ZoneID = 0; // by default a card is not in the attack zone
    }

    private void Update()
    {
        ManaCostText.text = ManaCost.ToString();
        AttackText.text = Attack.ToString();
        HealthText.text = Health.ToString();
    }

}
