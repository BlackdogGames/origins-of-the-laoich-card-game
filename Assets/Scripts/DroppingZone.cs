using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingZone : MonoBehaviour
{
    [SerializeField]
    public bool isPlayerZone;
    public bool isEnemyZone;

    public bool IsBeingUsed;


    public void Awake()
    {
        IsBeingUsed = false;
    }

}
