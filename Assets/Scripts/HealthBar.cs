using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public GameObject Player;
    PlayerStats _playerStats;
    public TMP_Text HealthText;

    // Update is called once per frame
    void Update()
    {
        _playerStats = Player.GetComponent<PlayerStats>();
        HealthText.text = "" + _playerStats.Health.ToString();
    }
}