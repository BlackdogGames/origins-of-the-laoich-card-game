using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{
    public PlayerStats Player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = (float)Player.Mana / (float)9;
    }
}
