using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private GameManager _gameManager;

    public TMP_Text TimerText;
    private float _turnTimer;
    public float TurnTime = 30;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _turnTimer = TurnTime;
    }

    private void Update()
    {
        if (!_gameManager.PlayersTurn)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true; 
        }

        _turnTimer -= Time.deltaTime;

        //display time remaining with no decimal place
        TimerText.text = ((int)_turnTimer).ToString();

        if (_turnTimer < 0)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        if (_gameManager.PlayersTurn) // if it is the players turn that is ending
        {
            // get all cards from field cards for player and if their card ability trigger is on turn end, invoke their ability
            foreach (GameObject card in _gameManager.Player.GetComponent<PlayerStats>().FieldCards)
            {
                if (card.GetComponent<CardStats>().CardAsset.AbilityTrigger == Card.CardAbilityTrigger.OnTurnEnd)
                {
                    card.GetComponent<CardStats>().CardAsset.Ability.Invoke(_gameManager, card.GetComponent<CardStats>());
                }
            }

            _gameManager.ManaIncrease(_gameManager.Player);

            _gameManager.PlayerStatsInstance.IsFirstTurn = false;

            foreach (var card in _gameManager.Player.GetComponent<PlayerStats>().FieldCards)
            {
                if (card != null)
                {
                    card.GetComponent<CardStats>().FirstTurnPlayed = false;
                    card.GetComponent<CardStats>().HasAttackedOpponent = false;
                }
                
            }

            // If any cards in opponent's field are invincible, set invincible to false
            foreach (var card in _gameManager.Opponent.GetComponent<PlayerStats>().FieldCards)
            {
                if (card != null)
                {
                    card.GetComponent<CardStats>().Invincible = false;
                }
            }

            if (_gameManager.PlayerStatsInstance.HandCards.Count < 7) // if the player has space, draw 1 new card
            {
                _gameManager.DrawCard(_gameManager.Player);
                // Calls AudioManager to Play requested sound effect.
                AudioManager.Instance.Play("SFX_Card_Pickup");
            }
            
        } 

        if (!_gameManager.PlayersTurn)// if it is the opponents turn that is ending
        {
            _gameManager.ManaIncrease(_gameManager.Opponent);
            _gameManager.OppStatsInstance.IsFirstTurn = false;
            
            foreach (var card in _gameManager.Opponent.GetComponent<PlayerStats>().FieldCards)
            {
                if (card != null)
                {
                    card.GetComponent<CardStats>().FirstTurnPlayed = false;
                    card.GetComponent<CardStats>().HasAttackedOpponent = false;
                }
            }
            
            // If any cards in players's field are invincible, set invincible to false
            foreach (var card in _gameManager.Player.GetComponent<PlayerStats>().FieldCards)
            {
                if (card != null)
                {
                    card.GetComponent<CardStats>().Invincible = false;
                }
            }

            if (_gameManager.OppStatsInstance.HandCards.Count < 7)
            {
                _gameManager.DrawCard(_gameManager.Opponent);
                // Calls AudioManager to Play requested sound effect.
                AudioManager.Instance.Play("SFX_Card_Pickup");
            }
            
        }

        _gameManager.ClearCardSelection(); // clear the card list

        _gameManager.PlayersTurn = !_gameManager.PlayersTurn; // End turn

        _turnTimer = TurnTime;
    }
}

