using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System;


[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    public int fallBoundary = -19;  //the player has fallen if he exceeds this y coordinate

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "GruntVoice";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats playerStats;

    private Rigidbody2D playerRigidbody;

    void Start()
    {

        playerStats = PlayerStats.instance;

        playerStats.currentHealth = playerStats.maxHealth;

        if (statusIndicator == null)
        {
            Debug.LogError("Player: No statusIndicator referenced on Player!");
        }
        else
        {
            statusIndicator.SetHealth(playerStats.currentHealth, playerStats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;  //the method OnUpgradeMenuToggle is now subscribed to the GameMaster's delegate onToggleUpgradeMenu and will be called when that delegate is invoked

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Player: No AudioManager found in the scene!");
        }

        InvokeRepeating("RegenHealth", 1f/playerStats.healthRegenRate, 1f/playerStats.healthRegenRate);
    }


    void RegenHealth()
    {
        playerStats.currentHealth += 1;
        statusIndicator.SetHealth(playerStats.currentHealth, playerStats.maxHealth);
    }


    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999999); //damaging the player infinitely to kill him for sure (he fell down)
        }

    }


    void OnUpgradeMenuToggle(bool active)
    {
        //handle what happens when the upgrade menu is toggled
        GetComponent<Platformer2DUserControl>().enabled = !active;  //if our upgrade menu is active, we don't want the Platformer2DUserControl script to be active
        
        if (this != null)
        {
            playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        }

        if (active == true)
        {
            playerRigidbody.velocity = Vector3.zero;  //disable movement of the player if he was in mid-air
            playerRigidbody.gravityScale = 0f;  //disable gravity of the player if he was in mid-air
        }
        else
        {
            playerRigidbody.gravityScale = 3f;
        }

        Weapon _weapon = GetComponentInChildren<Weapon>();  //disable/enable the weapon script when the upgrade menu is toggled
        if (_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }


    public void DamagePlayer(int damage)
    {
        playerStats.currentHealth -= damage;

        if (playerStats.currentHealth <= 0)
        {
            //Play death sound
            audioManager.PlaySound(deathSoundName);

            //Kill player
            GameMaster.KillPlayer(this);  //Kill the player after his health reaches or goes below zero
        }
        else
        {
            //Play damage sound
            audioManager.PlaySound(damageSoundName);
        }

        
        statusIndicator.SetHealth(playerStats.currentHealth, playerStats.maxHealth);
        
    }


    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
