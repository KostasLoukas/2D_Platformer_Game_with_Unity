using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int maxHealth = 100;
    private int _currentHealth;
    public int currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public float healthRegenRate = 2f;

    public float movementSpeed = 10f;


    void Awake()  //initializes variables
    {
        if (instance == null)
        {
            instance = this;
        }

    }

}