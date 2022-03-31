using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        _turnTimer -= Time.deltaTime;

        TimerText.text = "TIME REMAINING: " + _turnTimer;

        if (_turnTimer < 0)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        if (_gameManager.PlayersTurn) // if it is the players turn that is ending
        {
            _gameManager.ManaIncrease(_gameManager.Player);

            //TODO : This and the coupling line below should not work this way, something is wrong here
            _gameManager.OppStatsInstance.IsFirstTurn = false;

            foreach (var card in _gameManager.Opponent.GetComponent<PlayerStats>().Cards)
            {
                card.GetComponent<CardStats>().FirstTurnPlayed = false;
            }

            if (_gameManager.PlayerStatsInstance.Cards.Count < 7) // if the player has space, draw 1 new card
            {
                _gameManager.DrawCard(_gameManager.Player);
                // Calls AudioManager to Play requested sound effect.
                AudioManager.Instance.Play("SFX_Card_Pickup");
            }
            
        } 

        if (!_gameManager.PlayersTurn)// if it is the opponents turn that is ending
        {
            _gameManager.ManaIncrease(_gameManager.Opponent);
            _gameManager.PlayerStatsInstance.IsFirstTurn = false;

            if (_gameManager.OppStatsInstance.Cards.Count < 7)
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