using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaBar : MonoBehaviour
{
    public GameObject Player;
    PlayerStats _playerStats;
    public TMP_Text ManaText;

    // Update is called once per frame
    void Update()
    {
        _playerStats = Player.GetComponent<PlayerStats>();
        ManaText.text = "MANA: " + _playerStats.Mana.ToString();
    }
}