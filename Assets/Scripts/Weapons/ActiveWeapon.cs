using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Sword _sword;

    public static ActiveWeapon Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sword getActiveWeapon() { 
        return _sword;
    }
}
