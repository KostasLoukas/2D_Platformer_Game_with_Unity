using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;



[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }  //this ensures that health wil always be between 0 and maxHealth
        }

        public int damage = 30;

        public void Init()  //initializes variables
        {
            currentHealth = maxHealth;
        }

    }

    public EnemyStats enemyStats = new EnemyStats();
    [Header("Optional: ")]  //not all enemies *need* to have a statusIndicator...
    [SerializeField]
    private StatusIndicator statusIndicator;

    public Transform deathParticles;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";

    public int moneyDrop = 1;


    void Start()
    {
        enemyStats.Init();

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.currentHealth, enemyStats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;  //the method OnUpgradeMenuToggle is now subscribed to the GameMaster's delegate onToggleUpgradeMenu and will be called when that delegate is invoked

        if (deathParticles == null)
        {
            Debug.LogError("Enemy: No death particles referenced on Enemy!");
        }
    }


    void OnUpgradeMenuToggle(bool active)
    {
        //handle what happens when the upgrade menu is toggled
        GetComponent<EnemyAI>().enabled = !active;  //if our upgrade menu is active, we don't want the Platformer2DUserControl script to be active
        GetComponent<CircleCollider2D>().enabled = !active;
    }


    public void DamageEnemy(int damage)
    {
        enemyStats.currentHealth -= damage;

        if (enemyStats.currentHealth <= 0)
        {
            GameMaster.KillEnemy(this);  //Kill the player after his health reaches or goes below zero
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.currentHealth, enemyStats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colliderInfo)
    {
        Player _player = _colliderInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(enemyStats.damage);
            DamageEnemy(999999999);  //when the enemy collides with the player, they die
        }


    }


    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
